using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Diagnostics;
using System.Threading;

namespace TheMostGamesTask1
{
    /// <summary>
    /// MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

 
        JSONGetter jgetter = new JSONGetter(); //Class for getting JSONs from server.
        public delegate void UpdateTextDataGrid(DataGrid outputfield); //Delegate for updating output DataGrid.
        public delegate void UpdateButtonIsEnabled(Button button, bool state); //Delegate for updating request button state.
  

        public MainWindow()
        {
            InitializeComponent();
        }

        /*
         * Request button.
         */
        private async void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            bool incorrectInputFlag = false; //Flag for messagebox about incorrect string IDs.

            List<ProcessedElement> stringList = new List<ProcessedElement>(); //List of objects serving as item source for output DataGrid.
            List<int> IDList = new List<int>(); //List for request numbers
            List<int> IDListNoDuplicates = new List<int>(); //List fpr request numbers without correct, but duplicate values.

            /*
             * Clearing lists of values.
             */
            stringList.Clear();
            IDList.Clear();
            IDListNoDuplicates.Clear();

            /*
             * Block request button until request is finished.
             */
            var thisButton = ProcessButton;
            ProcessButton.Dispatcher.Invoke(new UpdateButtonIsEnabled(this.UpdateButton), new object[] { ProcessButton, false });

            StringTableGrid.ItemsSource = stringList;

            /*
             * Splitting input string into input values.
             */
            string fromRTB = new TextRange(InputTextBox.Document.ContentStart, InputTextBox.Document.ContentEnd).Text;
            fromRTB = fromRTB.Trim('\r', '\n',' '); //Removing end-of-string characters before splitting input text into input values.
            String[] sArray = fromRTB.Split(',', ';');

 
            /*
             * Clearing input string.
             */
            TextRange rangeOfText1 = new TextRange(InputTextBox.Document.ContentStart, InputTextBox.Document.ContentEnd);
            rangeOfText1.Text = "";
            rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);


            /*
            * This part filters incorrect input (anything, that is not numbers from 1 to 20).
            * During filtering initial input string is recreated with incorrect inputs being lighted up with red.
            */
            for (int i = 0; i < sArray.Length; i++)
            {
                try
                {
                    int parsedID = Int32.Parse(sArray[i]); //Trying to parse input values into integer.


                    if ((parsedID > 20) || (parsedID < 1)) //If number is not from 1 to 20 it's being ignored.
                    {
                        TextRange rangeOfText2 = new TextRange(InputTextBox.Document.ContentEnd, InputTextBox.Document.ContentEnd);
                        rangeOfText2.Text = sArray[i] + ",";
                        rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
                        rangeOfText2.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                        incorrectInputFlag = true;
                        continue;
                    }
                    else //if everything is OK, number is added to the list.
                    {
                        TextRange rangeOfText2 = new TextRange(InputTextBox.Document.ContentEnd, InputTextBox.Document.ContentEnd);
                        rangeOfText2.Text = sArray[i] + ",";
                        rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                        rangeOfText2.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
                        IDList.Add(parsedID);
                    }
                }
                catch (Exception exc) //if input is not decimal, it's being ignored.
                {
                    if ((sArray[i] != "\r\n") && (sArray[i] != ""))
                    {
                        TextRange rangeOfText2 = new TextRange(InputTextBox.Document.ContentEnd, InputTextBox.Document.ContentEnd);
                        rangeOfText2.Text = sArray[i] + ",";
                        rangeOfText2.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
                        rangeOfText2.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                        Console.WriteLine(exc.Message.ToString());
 
                        incorrectInputFlag = true;
                    }
                    
                    continue;
                }

            }

            /*
             * Sets input parameters of input RichTextBox back to normal.
             */
            TextRange rangeOfText3 = new TextRange(InputTextBox.Document.ContentEnd, InputTextBox.Document.ContentEnd);
            rangeOfText3.Text = " ";
            rangeOfText3.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
            rangeOfText3.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);

            /*
             * Removal of correct, but duplicate input numbers.
             */
            IDListNoDuplicates = IDList.Distinct().ToList();

            /*
             * Sending GET requests and getting back JSON with strings.
             */
            foreach (int stringID in IDListNoDuplicates)
            {
                await GetStringsAsync(stringID, stringList).ConfigureAwait(false);
            }

            /*
             * Refresh output DataGrid to show acquired strings.
             */
            StringTableGrid.Dispatcher.Invoke(new UpdateTextDataGrid(this.UpdateDataGridString), new object[] { StringTableGrid });

            

            /*
             * Unblock request button.
             */
            ProcessButton.Dispatcher.Invoke(new UpdateButtonIsEnabled(this.UpdateButton), new object[] { ProcessButton, true });

            if (incorrectInputFlag == true)
            {
                MessageBox.Show("Часть введённых идентификаторов строк некорректна.");
                incorrectInputFlag = false;
            }

        }

        /*
         * Task for getting JSONs async.
         */
        async Task GetStringsAsync(int stringID, List<ProcessedElement> stringList)
        {

            await Task.Run(async () =>
            {
                try
                {
                    /*
                     * Building url.
                     */
                    StringBuilder sURL = new StringBuilder();
                    sURL.Clear();
                    sURL.Append("http://tmgwebtest.azurewebsites.net/api/textstrings/");
                    sURL.Append(stringID.ToString());

                    var stringJSON = await jgetter.GetJSONAsync(sURL.ToString(), stringID); //Getting ClassForTextReceiving object with string.

                    if (stringJSON != null) //If everything is OK, adding string to item list.
                    {
                        stringList.Add(new ProcessedElement(stringJSON.Text));
                    }
                    else //Otherwise adding an error message with number of missed string to item list.
                    {
                        stringList.Add(new ProcessedElement("Error: string #" + stringID + " had not been found."));
                        Debug.WriteLine("Server connection error on receiving json");
                        Debug.WriteLine(jgetter.ErrorMessage);

                    }
                }
                catch (Exception exc) //If not possible to get JSON, add an connection error message with number of missed string to item list.
                {
                    stringList.Add(new ProcessedElement("Error: trying to get string #" + stringID + " has led to server connection error."));
                    Console.WriteLine(exc.Message.ToString());
                    Console.WriteLine("Exception on receiving json");
                    Debug.WriteLine(exc.Message.ToString());
                    Debug.WriteLine("Exception on receiving json");
                }
             });
        }

        /*
         * Function for updating output DataGrid from it's item list.
         */
        private void UpdateDataGridString(DataGrid datagrid)
        {
            datagrid.Items.Refresh();
        }

        /*
        * Function for updating request button state.
        */
        private void UpdateButton(Button button, bool state)
        {
            button.IsEnabled = state;
        }

        
    }
}
