using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjektBaza
{
    class TextBoxWithProp : TextBox {
        public string Prop;
        public TextBoxWithProp(string p)
        {
            Prop = p;
        }
    }


    class SubmitChangesButton : Button {
        private Product Target;
        public List<TextBoxWithProp> InputsGroup;

        public SubmitChangesButton(ref Product target)
        {
            Target = target;
            InputsGroup = new List<TextBoxWithProp>();
        }

        protected override void OnClick()
        {
            base.OnClick();
            string query = "update products set ";
            foreach (var input in InputsGroup)
            {
                var prop = Target.GetType().GetProperty(input.Prop);
                switch (prop.PropertyType.Name)
                {
                    case "String":
                        {
                            if (input.Prop != "Id")
                            {
                                query += input.Prop.ToLower() + " = '" + input.Text + "',";
                            }
                            prop.SetValue(Target, input.Text);
                            break;
                        }
                    case "Single":
                        {
                            if (input.Prop != "Id")
                            {
                                query += input.Prop.ToLower() + " = " + input.Text.Replace(',','.') + ",";
                            }
                            prop.SetValue(Target, float.Parse(input.Text));
                            break;
                        }
                    case "Int32":
                        {
                            if (input.Prop != "Id")
                            {
                                query += input.Prop.ToLower() + " = " + input.Text + ",";
                            }
                            prop.SetValue(Target, Convert.ToInt32(input.Text));
                            break;
                        }

                    default: break;
                }
            }
            query = query.Remove(query.Length - 1);
            query += " where id = " + Target.Id + ";";
            var connection = new MySqlConnection("datasource=127.0.0.1;port=3306;username=root;password=;database=projekt_baza");
            connection.Open();
            var command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            IsEnabled = false;

            MessageBox.Show("Zapisano zmiany");
        }

        public void StartObserving()
        {
            foreach (var i in InputsGroup)
            {
                i.TextChanged += Input_TextChanged;
            }
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsEnabled = true;
        }
    }


    class DataRow
    {
        private Product Target;
        private List<string> Props;
        private MainWindow MW;
        public DataRow(ref Product target, MainWindow mw)
        {
            Target = target;
            Props = new List<string>();
            foreach (PropertyInfo pInf in target.GetType().GetProperties())
            {
                if (pInf.Name == "ImageName" || pInf.Name == "Id") continue;
                Props.Add(pInf.Name);
            }
            MW = mw;
        }

        internal void Render(Grid grid, int height)
        {
            int i = grid.RowDefinitions.Count;
            int j = 0;
            grid.RowDefinitions.Add(new RowDefinition() {Height = new GridLength(height)});

            var submit = new SubmitChangesButton(ref Target);

            foreach (string prop in Props)
            {
                var l = new TextBlock();
                l.HorizontalAlignment = HorizontalAlignment.Center;
                l.Text = prop + ": ";
                grid.Children.Add(l);

                var f = new TextBoxWithProp(prop);
                f.Text = Target.GetType().GetProperty(prop).GetValue(Target)?.ToString() ?? "";
                f.Margin = new Thickness(20);
                grid.Children.Add(f);
                submit.InputsGroup.Add(f);

                Grid.SetRow(l, i);
                Grid.SetRow(f, i);

                Grid.SetColumn(l, j);
                Grid.SetColumn(f, j);
                j++;
            }

            submit.IsEnabled = false;
            submit.Content = "Zapisz zmiany";
            submit.Margin = new Thickness(30);
            submit.StartObserving();

            grid.Children.Add(submit);
            Grid.SetRow(submit, i);
            Grid.SetColumn(submit, j);

            var deleteButton = new Button();
            deleteButton.Content = "X";
            deleteButton.Background = Brushes.Red;
            deleteButton.Tag = Target.Id;
            deleteButton.Margin = new Thickness(80,30,80,30);
            Grid.SetRow(deleteButton, i);
            Grid.SetColumn(deleteButton, ++j);
            grid.Children.Add(deleteButton);

            deleteButton.Click += DeleteButton_Click;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            var connection = new MySqlConnection("datasource=127.0.0.1;port=3306;username=root;password=;database=projekt_baza");
            connection.Open();
            var command = new MySqlCommand("delete from products where id = " + b.Tag + ";", connection);
            command.ExecuteNonQuery();

            MW.products.Remove(Target);
            MW.RenderRows();
        }
    }
}
