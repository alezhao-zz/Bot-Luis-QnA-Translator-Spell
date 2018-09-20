namespace Microsoft.Bot.Sample.SimpleEchoBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Builder.Luis;
    using Microsoft.Bot.Builder.Luis.Models;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Sample.SimpleEchoBot.Entities;
    using Microsoft.Bot.Sample.SimpleEchoBot.Services;

    //[Application ID], [Authoring Key]
    [LuisModel("", "")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
        private const string EntityRestaurantPlan = "RestaurantReservation.PlaceName";
        private const string EntityRestaurantName = "RestaurantReservation.PlaceName";
        private const string EntityPlaceName = "Places.PlaceName";
        private const string EntityAddress = "RestaurantReservation.Address";
        private const string EntityRating = "RestaurantReservation.Rating";

        private string contentType = string.Empty;

        private IList<string> titleOptions = new List<string> { "“Very stylish great restaurant great staff”", "“good restaurant awful meals”", "“Need more attention to little things”", "“Lovely small hotel ideally situated to explore the area.”", "“Positive surprise”", "“Beautiful suite and resort”" };

        private readonly TranslatorService translatorService = new TranslatorService();
        private readonly QnAmakerService qnAmakerService = new QnAmakerService();

        [LuisIntent("RestaurantReservation.Reserve")]
        public async Task Reserve(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            contentType = message.Locale;

            await context.PostAsync(await translatorService.TranslatorExecute("en", contentType, $"Welcome to the Restaurant finder!"));
            // debug
            await context.PostAsync($"Translate to English: {message.Text}");
            

            var restaurantsQuery = new RestaurantsQuery();

            EntityRecommendation restaurantEntityRecommendation;

            if (result.TryFindEntity(EntityRestaurantPlan, out restaurantEntityRecommendation))
            {
                restaurantEntityRecommendation.Type = "PlaceName";
            }
            else if (result.TryFindEntity(EntityRestaurantName, out restaurantEntityRecommendation))
            {
                restaurantEntityRecommendation.Type = "PlaceName";
            }

            var restaurantsFormDialog = new FormDialog<RestaurantsQuery>(restaurantsQuery, BuildRestaurantsForm, FormOptions.PromptInStart, result.Entities);

            context.Call(restaurantsFormDialog, this.ResumeAfterRestaurantsFormDialog);
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            contentType = message.Locale;
            // debug
            await context.PostAsync($"Translate to English: {message.Text}");
            string returnMessage = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

            await context.PostAsync(await translatorService.TranslatorExecute("en", contentType, returnMessage));

            context.Wait(MessageReceived);
        }

        [LuisIntent("Places.GetReviews")]
        public async Task Reviews(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            EntityRecommendation restaurantEntityRecommendation;
            var qnaMakerResult = new QnAMakerResult();
            var qnaMakerResultTitle = new QnAMakerResult();
            var qnaMakerResultText = new QnAMakerResult();

            var message = await activity;
            contentType = message.Locale;
            // debug
            await context.PostAsync($"Translate to English: {message.Text}");
            if (result.TryFindEntity(EntityPlaceName, out restaurantEntityRecommendation))
            {
                await context.PostAsync(await translatorService.TranslatorExecute("en", contentType, $"Looking for reviews of '{restaurantEntityRecommendation.Entity}'..."));

                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();
                var randomCount = new Random();
                int count = randomCount.Next(1, 5);
                for (int i = 0; i < count; i++)
                {
                    var random = new Random(i);
                    qnaMakerResult = qnAmakerService.GetMessageFromQnAMaker("Review " + random.Next(6, 10).ToString());
                    qnaMakerResultTitle = qnAmakerService.GetMessageFromQnAMaker("Comments " + random.Next(1, 5).ToString());
                    qnaMakerResultText = qnAmakerService.GetMessageFromQnAMaker("Comments " + random.Next(6, 10).ToString());
                    ThumbnailCard thumbnailCard = new ThumbnailCard()
                    {
                        Title = await translatorService.TranslatorExecute("en", contentType, qnaMakerResultTitle.answers.Count > 0? qnaMakerResultTitle.answers[0].answer : titleOptions[random.Next(0, titleOptions.Count - 1)]),
                        Text = await translatorService.TranslatorExecute("en", contentType, qnaMakerResultText.answers.Count > 0 ? qnaMakerResultText.answers[0].answer : titleOptions[random.Next(0, titleOptions.Count - 1)]),
                        Images = new List<CardImage>()
                        {
                            new CardImage() { Url = qnaMakerResult.answers.Count > 0? "https://upload.wikimedia.org/wikipedia/en/e/ee/Unknown-person.gif" : qnaMakerResult.answers[0].answer}
                        },
                    };

                    resultMessage.Attachments.Add(thumbnailCard.ToAttachment());
                }

                await context.PostAsync(resultMessage);
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("RestaurantReservation.Help")]
        public async Task Help(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var qnaMakerResult = new QnAMakerResult();
            string returnMessage = string.Empty;
            var message = await activity;
            contentType = message.Locale;
            await context.PostAsync($"Translate to English: {message.Text}");
            qnaMakerResult = qnAmakerService.GetMessageFromQnAMaker(message.Text);
            message.Text = qnaMakerResult.answers.Count > 0 ? qnaMakerResult.answers[0].answer : string.Empty;
            // debug
            await context.PostAsync($"Get message from QnA maker: {message.Text}");
            if (!string.IsNullOrEmpty(message.Text))
                returnMessage = await translatorService.TranslatorExecute("en", contentType, message.Text);
            else
                returnMessage = await translatorService.TranslatorExecute("en", contentType, $"system error");
            await context.PostAsync(returnMessage);

            context.Wait(MessageReceived);
        }

        private IForm<RestaurantsQuery> BuildRestaurantsForm()
        {
            OnCompletionAsyncDelegate<RestaurantsQuery> processHotelsSearch = async (context, state) =>
            {
                var message = "Searching for restaurants";
                if (!string.IsNullOrEmpty(state.PlaceName))
                {
                    message += $" in {state.PlaceName}...";
                }
                else if (!string.IsNullOrEmpty(state.Address))
                {
                    message += $" near {state.Address.ToUpperInvariant()} address...";
                }

                await context.PostAsync(await translatorService.TranslatorExecute("en", contentType, message));
            };

            return new FormBuilder<RestaurantsQuery>()
                .Field(nameof(RestaurantsQuery.PlaceName), (state) => string.IsNullOrEmpty(state.PlaceName))
                .Field(nameof(RestaurantsQuery.Address), (state) => string.IsNullOrEmpty(state.Address))
                .OnCompletion(processHotelsSearch)
                .Build();
        }

        private async Task ResumeAfterRestaurantsFormDialog(IDialogContext context, IAwaitable<RestaurantsQuery> result)
        {
            try
            {
                var searchQuery = await result;
                var restaurants = GetRestaurantsAsync(searchQuery);
                // debug

                await context.PostAsync(await translatorService.TranslatorExecute("en", contentType, $"I found {restaurants.Count()} restaurants:"));

                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();

                foreach (var restaurant in restaurants)
                {
                    
                    HeroCard heroCard = new HeroCard()
                    {
                        Title = await translatorService.TranslatorExecute("en", contentType, restaurant.Name),
                        Subtitle = await translatorService.TranslatorExecute("en", contentType, $"{restaurant.Rating} stars. {restaurant.NumberOfReviews} reviews. From ${restaurant.PriceStarting} per night."),
                        Images = new List<CardImage>()
                        {
                            new CardImage() { Url = restaurant.Image }
                        },
                        Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = await translatorService.TranslatorExecute("en", contentType, "More details"),
                                Type = ActionTypes.OpenUrl,
                                Value = $"https://www.bing.com/search?q=restaurant+in+" + await translatorService.TranslatorExecute("en", contentType, HttpUtility.UrlEncode(restaurant.Location))
                            }
                        }
                    };

                    resultMessage.Attachments.Add(heroCard.ToAttachment());
                }

                await context.PostAsync(resultMessage);
            }
            catch (FormCanceledException ex)
            {
                string reply;

                if (ex.InnerException == null)
                {
                    reply = "You have canceled the operation.";
                }
                else
                {
                    reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
                }

                await context.PostAsync(await translatorService.TranslatorExecute("en", contentType, reply));
            }
            finally
            {
                context.Done<object>(null);
            }
        }

        private IEnumerable<Restaurant> GetRestaurantsAsync(RestaurantsQuery searchQuery)
        {
            var hotels = new List<Restaurant>();
            var qnaMakerResult = new QnAMakerResult();
            var randomCount = new Random();
            var count = randomCount.Next(1, 5);
            // Filling the hotels results manually just for demo purposes
            for (int i = 1; i <= count; i++)
            {
                var random = new Random(i);
                qnaMakerResult = qnAmakerService.GetMessageFromQnAMaker("Restaurant " + random.Next(1, 5).ToString());
                Restaurant restaurant = new Restaurant()
                {
                    Name = $"{searchQuery.PlaceName ?? searchQuery.Address} Restaurant {i}",
                    Location = searchQuery.PlaceName ?? searchQuery.Address,
                    Rating = random.Next(1, 5),
                    NumberOfReviews = random.Next(0, 5000),
                    PriceStarting = random.Next(80, 450),
                    Image = qnaMakerResult.answers.Count > 0 ? qnaMakerResult.answers[0].answer : string.Empty
                };

                hotels.Add(restaurant);
            }

            //hotels.Sort((h1, h2) => h1.PriceStarting.CompareTo(h2.PriceStarting));

            return hotels;
        }
    }
}
