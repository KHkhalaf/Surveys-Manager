using CrossPieCharts.FormsPlugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SurveyMonkey_Project.Models
{
    public class GetElements
    {
        public static StackLayout GetTextQuestion(string text, bool isEnabled, bool Actions)
        {
            StackLayout Stack = new StackLayout
            {
                Spacing = 15,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Button{ Text=Surveys_Manager.Lang.Resource.btnEdit,BackgroundColor = Color.FromHex("39f49d"),Image="edit.png"},
                    new Button{ Text=Surveys_Manager.Lang.Resource.deleteSurveytoolbar,BackgroundColor = Color.FromHex("dfc9ad"),Image="delete.png"}
                }
            };
            
            StackLayout stack = new StackLayout
            {
                StyleId = "text",
                Spacing = 10,
                Padding = new Thickness(5, 0, 5, 5),
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White,
                Children = {
                    new Label { BackgroundColor = Color.FromHex("1fe887"), HeightRequest = 4 },
                    new Label { Text = text, FontSize = 20 },
                    new Entry {  BackgroundColor = Color.FromHex("f4f4f4") , IsEnabled = isEnabled}
                }
            };
            if (Actions)
            {
                stack.Children.Add(new Label { HeightRequest =2,BackgroundColor = Color.FromHex("1fe887") });
                stack.Children.Add(Stack);
            }
            return stack;
        }
        public static Grid GetAnswerChoics(string text)
        {

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            var entry = new Entry { Text = text, Placeholder = Surveys_Manager.Lang.Resource.Enter_A_Choice, HorizontalOptions = LayoutOptions.FillAndExpand };
            var img = new Image { Source = "cancel1.png", HorizontalOptions = LayoutOptions.End };
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (sender, e) => { grid.IsVisible = false; };
            img.GestureRecognizers.Add(tap);

            Grid.SetColumn(entry, 0);
            Grid.SetColumn(img, 1);

            grid.Children.Add(entry);
            grid.Children.Add(img);
            return grid;
        }
        public static StackLayout GetSlider(int min, int max, string text, bool isEnabled, bool Actions)
        {
            StackLayout Stack = new StackLayout
            {
                Spacing = 15,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Button{ Text=Surveys_Manager.Lang.Resource.btnEdit,BackgroundColor = Color.FromHex("39f49d"),Image="edit.png"},
                    new Button{ Text=Surveys_Manager.Lang.Resource.deleteSurveytoolbar,BackgroundColor = Color.FromHex("dfc9ad"),Image="delete.png"}
                }
            };

            Label label = new Label { Text = max.ToString(), HorizontalOptions = LayoutOptions.End };
            Slider slider = new Slider
            {
                Maximum = max,
                Minimum = min,
                Value = min,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                IsEnabled = isEnabled
            };
            slider.ValueChanged += (sender, e) => {
                if (slider.Value.ToString().Contains("."))
                    label.Text = slider.Value.ToString().Substring(0, slider.Value.ToString().IndexOf("."));
                else
                    label.Text = slider.Value.ToString();
            };
            StackLayout stack = new StackLayout
            {
                StyleId = "slider",
                Spacing = 5,
                Padding = new Thickness(5, 0, 5, 5),
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White,
                Children = {
                    new Label { BackgroundColor = Color.FromHex("1fe887"), HeightRequest = 4 },
                    new Label { Text = text, FontSize = 20 },
                    new StackLayout{
                        Spacing = 5,
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            new Label{ Text = min.ToString(),HorizontalOptions = LayoutOptions.Start},
                        slider ,
                        label
                        }
                    },
                }
            };
            if (Actions)
            {
                stack.Children.Add(new Label { HeightRequest = 2, BackgroundColor = Color.FromHex("1fe887") });
                stack.Children.Add(Stack);
            }
            return stack;
        }
        public static StackLayout GetDropDownQuestion(string textquestion, List<string> choices, bool isEnabled, bool Actions)
        {
            StackLayout Stack = new StackLayout
            {
                Spacing = 15,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Button{ Text=Surveys_Manager.Lang.Resource.btnEdit,BackgroundColor = Color.FromHex("39f49d"),Image="edit.png"},
                    new Button{ Text=Surveys_Manager.Lang.Resource.deleteSurveytoolbar,BackgroundColor = Color.FromHex("dfc9ad"),Image="delete.png"}
                }
            };

            Picker P = new Picker() { BackgroundColor = Color.FromHex("f4f4f4"), Title = " Choices", IsEnabled = isEnabled };
            for (int i = 0; i < choices.Count; i++)
                P.Items.Add(choices.ElementAt(i));
            P.SelectedIndexChanged += (sender, e) => {
               P.Title = P.Items[P.SelectedIndex];
            };
            StackLayout stack = new StackLayout
            {
                StyleId = "dropdown",
                Spacing = 10,
                Padding = new Thickness(5, 0, 5, 5),
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White,
                Children = {
                    new Label { BackgroundColor = Color.FromHex("1fe887"), HeightRequest = 4 },
                    new Label { Text = textquestion, FontSize = 20 },
                    P
                }
            };
            if (Actions)
            {
                stack.Children.Add(new Label { HeightRequest = 2, BackgroundColor = Color.FromHex("1fe887") });
                stack.Children.Add(Stack);
            }
            return stack;
        }
        public static StackLayout Get_Radio_or_CheckBox(bool checkbox_Not_radio, string[] choices, bool isEnabled)
        {
            Label[] Text = new Label[choices.Length];// { Text = text , FontSize = 15};
            Image[] img = new Image[choices.Length];// { Scale = 1.5, IsEnabled = isEnabled };
            StackLayout[] stack = new StackLayout[choices.Length]; //{ Orientation = StackOrientation.Horizontal, Spacing = 20, };
            StackLayout stackTotal = new StackLayout();
            for (int i = 0; i < choices.Length; i++)
            {
                Text[i] = new Label { Text = choices[i], FontSize = 15 };
                img[i] = new Image { Scale = 1.5, IsEnabled = isEnabled };
                if (checkbox_Not_radio)
                {
                    img[i].Source = "uncheckedcheckbox.png";
                    img[i].StyleId = "uncheckedcheckbox";
                }
                else if (!checkbox_Not_radio)
                {
                    img[i].Source = "uncheckedradio.png";
                    img[i].StyleId = "uncheckedradio";
                }
                //TapGestureRecognizer tap = new TapGestureRecognizer();
                //tap.Tapped += (sender, e) => {
                //    if (img[i].StyleId.Contains("checkbox"))
                //    {
                //        if (img[i].StyleId.Contains("un"))
                //        {
                //            img[i].Source = "checkedcheckbox.png";
                //            img[i].StyleId = "checkedcheckbox";
                //        }
                //        else
                //        {
                //            img[i].Source = "uncheckedcheckbox.png";
                //            img[i].StyleId = "uncheckedcheckbox";
                //        }

                //    }
                //    else if (img[i].StyleId.Contains("radio"))
                //    {
                //        if (img[i].StyleId.Contains("un"))
                //        {
                //            img[i].Source = "checkedradio.png";
                //            img[i].StyleId = "checkedradio";
                //            for (int j = 0; j < choices.Length && j != i; j++)
                //            {
                //                img[i].Source = "uncheckedradio.png";
                //                img[i].StyleId = "uncheckedradio";
                //            }
                //        }
                //        else
                //        {
                //            img[i].Source = "uncheckedradio.png";
                //            img[i].StyleId = "uncheckedradio";
                //        }

                //    }
                //};
                //img[i].GestureRecognizers.Add(tap);
                stack[i] = new StackLayout
                {
                    IsEnabled = isEnabled,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 20,
                    Children = { img[i], Text[i] }
                };
                stackTotal.Children.Add(stack[i]);
            }
            return stackTotal;
        }
        public static StackLayout GetMultiChoices(bool checkbox_Not_radio, string textquestion, string[] choices, bool isEnabled,bool Actions)
        {

            StackLayout Stack = new StackLayout
            {
                Spacing = 15,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Button{ Text=Surveys_Manager.Lang.Resource.btnEdit,BackgroundColor = Color.FromHex("39f49d"),Image="edit.png"},
                    new Button{ Text=Surveys_Manager.Lang.Resource.deleteSurveytoolbar,BackgroundColor = Color.FromHex("dfc9ad"),Image="delete.png"}
                }
            };

            StackLayout stack = new StackLayout
            {
                BackgroundColor = Color.White,
                Spacing = 5,
                Padding = new Thickness(10, 10, 0, 10)
            };

            stack.Children.Add(Get_Radio_or_CheckBox(checkbox_Not_radio, choices, isEnabled));
            for (int i = 0; i < (stack.Children[0] as StackLayout).Children.Count; i++)
            {
                TapGestureRecognizer tap = new TapGestureRecognizer();
                var img = ((stack.Children[0] as StackLayout).Children[i] as StackLayout).Children[0] as Image;
                tap.Tapped += (sender, e) =>
                {
                    if (img.StyleId.Contains("checkbox"))
                    {
                        if (img.StyleId.Contains("un"))
                        {
                            img.Source = "checkedcheckbox.png";
                            img.StyleId = "checkedcheckbox";
                        }
                        else
                        {
                            img.Source = "uncheckedcheckbox.png";
                            img.StyleId = "uncheckedcheckbox";
                        }

                    }
                    else if (img.StyleId.Contains("radio"))
                    {
                        if (img.StyleId.Contains("un"))
                        {
                            for (int j = 0; j < (stack.Children[0] as StackLayout).Children.Count ; j++)
                            {
                                var img1 = ((stack.Children[0] as StackLayout).Children[j] as StackLayout).Children[0] as Image;
                                img1.Source = "uncheckedradio.png";
                                img1.StyleId = "uncheckedradio";
                            }
                            img.Source = "checkedradio.png";
                            img.StyleId = "checkedradio";
                        }
                        else
                        {
                            img.Source = "uncheckedradio.png";
                            img.StyleId = "uncheckedradio";
                        }

                    }
                };
                img.GestureRecognizers.Add(tap);
            }

            StackLayout stackTotal = new StackLayout
            {
                StyleId = "multible",
                Spacing = 5,
                Padding = new Thickness(5, 0, 5, 5),
                BackgroundColor = Color.White,
                Children = {
                    new Label { BackgroundColor = Color.FromHex("1fe887"), HeightRequest = 4 },
                    new Label { Text = textquestion, FontSize = 20 },
                    stack
                }
            };
            if (Actions)
            {
                stackTotal.Children.Add(new Label { HeightRequest = 2, BackgroundColor = Color.FromHex("1fe887") });
                stackTotal.Children.Add(Stack);
            }
            return stackTotal;
        }
        public static StackLayout GetAnswers_byUsers(string choice, string userName)
        {
            StackLayout stack = new StackLayout() {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label{ Text = "  "+choice, FontSize = 17, HorizontalOptions = LayoutOptions.StartAndExpand},
                    new Label{ Text = Surveys_Manager.Lang.Resource.by, FontSize = 17, HorizontalOptions = LayoutOptions.End},
                    new Label{ Text = " "+userName, TextColor = Color.FromHex("4597f3"), FontSize = 18 , FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.End}
                }
            };
            return stack;
        }
        public static StackLayout GetAnswers_byUsers_forMulible(int progress, int j, List<User> userNames, string choice)
        {
            StackLayout stackTotal = new StackLayout()
            {
                Spacing = 4,
                Children = {
                    new Label{ Text = j+"- "+choice, FontSize = 19, HorizontalOptions = LayoutOptions.StartAndExpand},
                }
            };
            for (int i = 0; i < userNames.Count; i++)
            {
                StackLayout stack = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        new Label{ Text = "- "+Surveys_Manager.Lang.Resource.AnsweredBy, FontSize = 17, HorizontalOptions = LayoutOptions.StartAndExpand},
                        new Label{ Text = " "+userNames.ElementAt(i).UserName, TextColor = Color.FromHex("4597f3"), FontSize = 18 , FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.End}
                    }
                };
                stackTotal.Children.Add(stack);
            }

            stackTotal.Children.Add(
            new Grid
            {
                Children ={
                                    new CrossPieChartView
                                    {
                                        Progress = progress,
                                        ProgressColor = Color.Green,
                                        ProgressBackgroundColor = Color.FromHex("#EEEEEEEE"),
                                        StrokeThickness = Device.OnPlatform(10, 20, 80),
                                        Radius = Device.OnPlatform(100, 180, 160)
                                    },
                                    new Label
                                    {
                                        Text = progress+"%",
                                        Font = Font.BoldSystemFontOfSize(NamedSize.Large),
                                        FontSize = 70,
                                        VerticalOptions = LayoutOptions.Center,
                                        HorizontalOptions = LayoutOptions.Center,
                                        TextColor = Color.Black
                                    }
                        }
            });
            stackTotal.Children.Add(new Label { BackgroundColor = Color.FromHex("a7cefb"), HeightRequest = 2 });
            return stackTotal;
        }
    }
}
