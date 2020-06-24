using System;
using System.IO;
using System.Net.Mail;
using System.Windows.Forms;
namespace Assignment4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        const int TIMEFRAME1 = 1, TIMEFRAME2 = 3, TIMEFRAME3 = 5, TIMEFRAME4 = 10;
        int Term,TotalTerm,TotalTransaction;
        bool EmailStatus;
        decimal InvestedAmount,TotalAmount,InterestRate;
        decimal EndAmount1, EndAmount2, EndAmount3, EndAmount4,FinalAmount,FinalInterest,FinalValue;
        string ClientName, ClientTelNo, ClientMailId, TransactionNo, Email;
        const decimal LINTERESTRATE1 = 0.50000m, LINTERESTRATE2 = 0.62500m, LINTERESTRATE3 = 0.71250m, LINTERESTRATE4 = 1.01250m,
                GINTERESTRATE1 = 0.60000m, GINTERESTRATE2 = 0.72500m, GINTERESTRATE3 = 0.81250m, GINTERESTRATE4 = 1.02500m;
        
        //Handles Exception if values are too big or null.
        private void InvestAmountTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {   //Checks if the value is too big.
                int.Parse(InvestAmountTextBox.Text);
                //Sub checks for displaying bonus label.
                    if (InvestAmountTextBox.Text != "" && int.Parse(InvestAmountTextBox.Text) > 1000001)
                    {
                        BonusDetailLabel.Visible = true;
                        BonusDetailLabel.Text = "Eligible for 25000 euro bonus if term is greater than 1";
                    }
                    else if ((InvestAmountTextBox.Text) == "" || (int.Parse(InvestAmountTextBox.Text) < 25000))
                        BonusDetailLabel.Visible = false;
            }
            catch
            {
                //Exception is thrown.
                if (string.IsNullOrEmpty(InvestAmountTextBox.Text))
                    MessageBox.Show("Input Field","Empty Field",MessageBoxButtons.OK,MessageBoxIcon.Error);
                else
                {
                    MessageBox.Show("Enter a Smaller/Numerical value");
                    InvestAmountTextBox.SelectAll();
                }
            }
        }

        
        //Exits the application.
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Sets the form to defaulr state for another input by setting the visible and enable accordingly.
        private void ClearButton_Click(object sender, EventArgs e)
        {
            SearchTrxNoGroupBox.Visible = false;
            MailIdDisplayListView.Visible = false;
            DisplayButton.Enabled = true;
            InvestAmountTextBox.Focus();
            TimeFrameListBox.Visible = false ;
            SummaryGroupBox.Visible = false;
            SummaryButton.Enabled = true;
            SummaryButton.Visible = true;
            ClientDetailGroupBox.Visible = false; ;
            DetailsDisplayGroupBox.Visible = false;
            SearchDisplayGroupBox.Visible = false;
            InvestAmountTextBox.Enabled = true;
            SearchTrxNoGroupBox.Visible = false;
            SearchMailIdGroupBox.Visible = false;
            
            SearchOptionGroupBox.Visible = true;
            FirstProceedButton.Visible = false;
            ConfirmButton.Visible = false;
            FinalConfirmButton.Visible = false;
            InvestAmountTextBox.Clear();
            InvestAmountTextBox.Focus();
            ClearButton.Enabled = false;
            TrnxNoSearchRadioButton.Checked = false;
            MailIdSearchRadioButton.Checked = false;
        }
        //Handling the radio buttons and searchbox visibility.
        private void TrnxNoSearchRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SearchMailIdGroupBox.Visible = false;
            SearchTrxNoGroupBox.Visible = true;
        }
        private void MailIdSearchRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SearchMailIdGroupBox.Visible = true;
            SearchTrxNoGroupBox.Visible = false;
        }


        //When users confirms his final details and books his investment.
        private void FinalConfirmButton_Click(object sender, EventArgs e)
        {
            InvestedAmount = decimal.Parse(InvestAmountTextBox.Text);

        //If all the fields are are there and valid then it will write it in the file.    
            if (ClientName != "" && ClientTelNo != "" && EmailStatus == true)
            {
                //Creates object for writing in file
                StreamWriter InvestMeFile;

                //Opens the file in append mode and writes.
                InvestMeFile = File.AppendText("InvestMeTransactionDetails.txt");
              
                InvestMeFile.WriteLine("xxxxxxxx");
                InvestMeFile.WriteLine(TransactionNo);
                InvestMeFile.WriteLine(ClientMailId);
                InvestMeFile.WriteLine(ClientName);
                InvestMeFile.WriteLine(ClientTelNo);
                InvestMeFile.WriteLine(InvestedAmount);
                InvestMeFile.WriteLine(Term);
                InvestMeFile.WriteLine(InterestRate);
                InvestMeFile.WriteLine(FinalAmount);               
                InvestMeFile.Close();
               
            }
            //Sets the value back to 0 if user wishes to view the summary in process after confirmation.
            SummaryListView.Items.Clear();
            TotalTerm = 0;
            TotalAmount = 0;
            FinalInterest = 0;
          
            MessageBox.Show("Transaction No:" + TransactionNo + "\n\nBOOKING IS CONFIRMED" , "Thankyou For Investing with us.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            //Visibility is set if user wants to proceed with another transaction/Summary/Search.
            TimeFrameListBox.Visible = false;
            InvestAmountTextBox.Enabled = true;
            DetailsDisplayGroupBox.Visible = false;
            ClientDetailGroupBox.Visible = false;
            FirstProceedButton.Visible = false;
            ConfirmButton.Visible = false;
            FinalConfirmButton.Visible = false;
            InvestAmountGroupBox.Enabled = true;
            InvestAmountTextBox.Clear();
            InvestAmountTextBox.Focus();
            DisplayButton.Enabled = true;
            SummaryButton.Enabled = true;
            
        }
        //Checks if the Email Id is in valid format or not.
        bool IsValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Summary box is displayed with the summary of all the transactions.
        private void SummaryButton_Click(object sender, EventArgs e)
        {
            SummaryButton.Enabled = false;
            ClearButton.Enabled = true;
           // decimal TempInterest = 0m;
            SummaryGroupBox.Visible = true;
            SummaryListView.Items.Clear();
            //Opens the file in read mode.
            StreamReader InvestMeFile;
            InvestMeFile = File.OpenText("InvestMeTransactionDetails.txt");
            while (!InvestMeFile.EndOfStream)
            {   //Stores all the data in temp variables if it has to be used to display.           
                string x1 = InvestMeFile.ReadLine();
                string TempTrxnNo = InvestMeFile.ReadLine();
                string TempEmailId = InvestMeFile.ReadLine();
                string TempClientName = InvestMeFile.ReadLine();
                string TempTelNo = InvestMeFile.ReadLine();
                string TempPrincipalAmount = InvestMeFile.ReadLine();
                string TempTerm = InvestMeFile.ReadLine();
                string TempInterestRate = InvestMeFile.ReadLine();
                string TempFinalAmount = InvestMeFile.ReadLine();
                //Checks and increments for total number of transactions.
                if(TempTrxnNo!="")
                {
                    TotalTransaction++;
                }
                //Increments the global variable value for summary display.
                TotalAmount += decimal.Parse(TempPrincipalAmount);
                TotalTerm += int.Parse(TempTerm);
                FinalValue+=  decimal.Parse(TempFinalAmount);             
                //Calls meathod to add items and display the summary listview.
                SummaryDisplay(TempTrxnNo);
                
            }
            //Closes File.
            InvestMeFile.Close();
            FinalInterest = FinalValue - TotalAmount;
            InvestedAmountSummaryDisplayLabel.Text = TotalAmount.ToString("C4");
            TotalTermsSummaryDisplayLabel.Text = TotalTerm.ToString();
            InterestSummaryDisplayLabel.Text = FinalInterest.ToString("C4");
            TotalTransactionsDisplaySummaryLabel.Text = TotalTransaction.ToString();

        }
        //Meathod to Display items in Summary listview.
        private void SummaryDisplay(string TempTrxnNo)
        {
            //SummaryListView.Items.Clear();
            ListViewItem list1 = new ListViewItem(TempTrxnNo);
            SummaryListView.Items.Add(list1);
        }

        //Confirm button confirms the user details that are Entered.  
        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            //Calls the meathod to validat Email-Id.
            EmailStatus = IsValidEmail(ClientMailIdTextBox.Text);
            ClientName = ClientNameTextBox.Text;
            ClientTelNo = ClientTelNoTextBox.Text;
            ClientMailId = ClientMailIdTextBox.Text;
            TransactionNo = TransactionNoDisplayLabel.Text;
            //Check if all fields are valid and displays accordingly.
            if (ClientName != "" && ClientTelNo != "" && ClientMailId != "")
            {
                if (EmailStatus == true)
                {
                    ClientDetailGroupBox.Visible = false;
                    ConfirmButton.Enabled = false;
                    DetailsDisplayGroupBox.Visible = true;
                    FinalConfirmButton.Visible = true;
                    FinalConfirmButton.Enabled = true;
                    switch (TimeFrameListBox.SelectedIndex)
                    {
                        case 0:
                            Term = TIMEFRAME1;
                            break;
                        case 1:
                            Term = TIMEFRAME2;
                            break;
                        case 2:
                            Term = TIMEFRAME3;
                            break;
                        case 3:
                            Term = TIMEFRAME4;
                            break;
                    }
                    //Displays the details in details box.
                    ClientDisplayDetailsLabel.Text = ClientName;
                    ClientTelNoDisplayDetailsLabel.Text = ClientTelNo;
                    ClientMailIdDisplayDetailsLabel.Text = ClientMailId;
                    TransactionNoDisplayDetailsLabel.Text = TransactionNo;
                    decimal TempPrincipalAmount = decimal.Parse(InvestAmountTextBox.Text);
                    PrincipalAmountDisplayDetailsLabel.Text = TempPrincipalAmount.ToString("C4");
                    InterestRateDisplayDetailsLabel.Text = InterestRate.ToString("0.00");
                    FinalAmountDisplayDetailsLabel.Text = FinalAmount.ToString("C4");
                    TermDisplayDetailsLabel.Text = Term.ToString();
                }
                else
                {
                    //If boolean value is false.
                    MessageBox.Show("Please Enter a valid Email ID", "Invalid Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClientMailIdTextBox.Focus();
                    ClientMailIdTextBox.SelectAll();
                }
            }
            else
                 MessageBox.Show("Enter all fields", "Invalid/Blank Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
       
        //Handles exception if term is not selected with proceed button enability.
        private void TimeFrameListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TimeFrameListBox.SelectedIndex != -1)            
                FirstProceedButton.Enabled = true;  
                
            else
                MessageBox.Show("Select a term","Invalid Selection",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        //Search by MailId.
        private void MailIdSearchButton_Click(object sender, EventArgs e)
        {
            MailIdDisplayListView.Items.Clear();
            bool Status = false;
            StreamReader InvestMeFile;
            InvestMeFile = File.OpenText("InvestMeTransactionDetails.txt");
            while (!InvestMeFile.EndOfStream)
            {
                string temp= InvestMeFile.ReadLine();
                string TempTrxnNo = InvestMeFile.ReadLine();
                string TempEmailId = InvestMeFile.ReadLine();
                string TempClientName = InvestMeFile.ReadLine();
                string TempTelNo = InvestMeFile.ReadLine();
                string TempPrincipalAmount = InvestMeFile.ReadLine();
                string TempTerm = InvestMeFile.ReadLine();
                string TempInterestRate = InvestMeFile.ReadLine();
                string TempFinalAmount = InvestMeFile.ReadLine();
                //If mail ID matches.
                if (TempEmailId == MailIdSearchTextBox.Text)
                {
                    Status = true;
                    ClearButton.Enabled = true;
                    MailIdDisplayListView.Visible = true;
                    //Calls a meathod to display in a listview.
                    IdSearchDisplay(TempTrxnNo, TempClientName, TempTelNo,decimal.Parse(TempPrincipalAmount),decimal.Parse(TempInterestRate),decimal.Parse(TempFinalAmount),TempTerm);                   
                }               
            }
            if (Status == false)
                MessageBox.Show("Email ID not found", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            InvestMeFile.Close();
        }
        //Created a meathod to display the Details if Mail ID matches.
        private void IdSearchDisplay(string TempTrxnNo, string TempClientName, string TempTelNo,decimal TempPrincipalAmount,decimal TempInterestRate,decimal TempFinalAmount,string TempTerm)
        {
            
            ListViewItem list1 = new ListViewItem(TempTrxnNo);
            list1.SubItems.Add(TempClientName);
            list1.SubItems.Add(TempTelNo);
            list1.SubItems.Add(TempPrincipalAmount.ToString("C4"));
            list1.SubItems.Add(TempInterestRate.ToString("C3"));
            list1.SubItems.Add(TempFinalAmount.ToString("C4"));
            list1.SubItems.Add(TempTerm);
            MailIdDisplayListView.Items.Add(list1);
        }

        //Search by Transaction ID.
        private void TransactionIDSearchButton_Click(object sender, EventArgs e)
        {

            bool Status = false;
            StreamReader InvestMeFile;
            InvestMeFile = File.OpenText("InvestMeTransactionDetails.txt");
            while (!InvestMeFile.EndOfStream)
            {   string x1= InvestMeFile.ReadLine();
                string TempTrxnNo = InvestMeFile.ReadLine();
                string TempEmailId = InvestMeFile.ReadLine();
                string TempClientName = InvestMeFile.ReadLine();
                string TempTelNo = InvestMeFile.ReadLine();
                string TempPrincipalAmount = InvestMeFile.ReadLine();
                string TempTerm = InvestMeFile.ReadLine();
                string TempInterestRate = InvestMeFile.ReadLine();
                string TempFinalAmount = InvestMeFile.ReadLine();
                //Checks if Transaction No is there or not.
                if (TempTrxnNo == TransactionNoSearchTextBox.Text)
                {
                    //bool is set to true if transaction id if found and calls meathod to display the details.
                    Status = true;
                    SearchDisplayGroupBox.Visible = true;
                    //Calls the Meathod to display
                    NoSearchDisplay(TempTrxnNo, TempEmailId, TempClientName, TempTelNo,decimal.Parse(TempPrincipalAmount),decimal.Parse(TempInterestRate),decimal.Parse(TempFinalAmount),TempTerm);
                }
            }
            
            if (Status == false)
                MessageBox.Show("Transaction ID not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            InvestMeFile.Close();
        }
        private void NoSearchDisplay(string TempTrxnNo, string TempEmailId, string TempClientName, string TempTelNo,decimal TempPrincipalAmount,decimal TempInterestRate,decimal TempFinalAmount,string TempTerm)
        {
            //Displays the details with required formatting.
            NameSearchDisplayLabel.Text = TempClientName;
            TelNoSearchDisplayLabel.Text = TempTelNo;
            MailIdSearchDisplayLabel.Text = TempEmailId;
            TransactionNoSearchDisplayLabel.Text = TempTrxnNo;
            PrincipalAmountIdSearchLabel.Text = TempPrincipalAmount.ToString("C4");
            InterestRateSearchIdDisplayLabel.Text = TempInterestRate.ToString("C3");
            FinalAmountSearchIdDisplayLabel.Text = TempFinalAmount.ToString("C4");
            SearchIdTermDisplayLabel.Text = TempTerm;
        }

        //After a term is selected from list box.
        private void FirstProceedButton_Click(object sender, EventArgs e)
        {
            TimeFrameListBox.Visible = false;
            FirstProceedButton.Enabled = false;
            ClientDetailGroupBox.Visible = true;
            ConfirmButton.Visible = true;
            ConfirmButton.Enabled = true;
            int Number = 0;
            string temp;
            //Random 6 character Transaction ID is generated. 
            temp = RandomGenerate(Number).ToString();
            ClientDetailGroupBox.Visible = true;
            Email = ClientMailIdTextBox.Text;
            if (InvestedAmount <= 250000)
            {
                //Stores the value according to the above if condition.
                switch (TimeFrameListBox.SelectedIndex)
                {
                    case 0:
                        Term = TIMEFRAME1;
                        InterestRate = LINTERESTRATE1;
                        FinalAmount = EndAmount1;
                        break;
                    case 1:
                        Term = TIMEFRAME2;
                        InterestRate = LINTERESTRATE2;
                        FinalAmount = EndAmount2;
                        break;
                    case 2:
                        Term = TIMEFRAME3;
                        InterestRate = LINTERESTRATE3;
                        FinalAmount = EndAmount3;
                        break;
                    case 3:
                        Term = TIMEFRAME4;
                        InterestRate = LINTERESTRATE4;
                        FinalAmount = EndAmount4;
                        break;
                }
            }
            else
            {

                switch (TimeFrameListBox.SelectedIndex)
                {
                    case 0:
                        Term = TIMEFRAME1;
                        InterestRate = GINTERESTRATE1;
                        FinalAmount = EndAmount1;
                        break;
                    case 1:
                        Term = TIMEFRAME2;
                        InterestRate = GINTERESTRATE2;
                        FinalAmount = EndAmount2;
                        break;
                    case 2:
                        Term = TIMEFRAME3;
                        InterestRate = GINTERESTRATE3;
                        FinalAmount = EndAmount3;
                        break;
                    case 3:
                        Term = TIMEFRAME4;
                        InterestRate = GINTERESTRATE4;
                        FinalAmount = EndAmount4;
                        break;
                }
            }
            //Checks if the file exists already.
            if (File.Exists("InvestMeTransactionDetails.txt"))
            {
                //Reads the file and Checks if the transaction ID is unique or not.
                StreamReader InvestMeFile;
                InvestMeFile = File.OpenText("InvestMeTransactionDetails.txt");
                while (!InvestMeFile.EndOfStream)
                {
                    InvestMeFile.ReadLine();                    
                    string x = InvestMeFile.ReadLine();
                    InvestMeFile.ReadLine();
                    InvestMeFile.ReadLine();
                    InvestMeFile.ReadLine();
                    string TempInterestRate = InvestMeFile.ReadLine();
                    string TempFinalAmount = InvestMeFile.ReadLine();
                    //If transaction ID exists meathod is called again to generate another ID;
                    if (x == temp)
                            temp = RandomGenerate(Number).ToString();                    
                }
                TransactionNoDisplayLabel.Text = temp;
                InvestMeFile.Close();

            }
            //If file doesnt exists it will just create a file for future use.
            else
            {
                int TransactionNo = RandomGenerate(Number);
                ClientDetailGroupBox.Visible = true;
                TransactionNoDisplayLabel.Text = TransactionNo.ToString();
                StreamWriter InvestMeFile;
                InvestMeFile = File.CreateText("InvestMeTransactionDetails.txt");
            }
            ClientNameTextBox.Clear();
            ClientMailIdTextBox.Clear();
            ClientTelNoTextBox.Clear();
            ClientNameTextBox.Focus();
        }
        //Return Type Meathod is created to generate Random 6 char digits. 
        private int RandomGenerate(int RandomNo)
        {

            Random rand = new Random();
            return RandomNo = rand.Next(99999, 999999) + 1; ;

        }
        //Calculates compound interest using math functions.
        private decimal CalculateInterest(decimal PrincipalAmount, decimal Rate, int Term)
        {
            //According to C.I formulae P(1+R/100n)^t where n=12 as it is compunded monthly.
            decimal EndAmount = PrincipalAmount * (decimal)Math.Pow((double)(1 + (Rate / 1200)), (double)Term * 12);
            if (PrincipalAmount > 100000 && Term>1)
                EndAmount += 25000;
            return EndAmount;
        }
        //Displays the available terms and interests rates in a list box according to the user investment value.
        private void DisplayButton_Click(object sender, EventArgs e)
        {
            //By-deafault checked=false may not work thus syntaxing it here. 
            TrnxNoSearchRadioButton.Checked = false;
            decimal  PrincipalAmount;
            try
            {
                //If value is parsed visibility is handled.
                PrincipalAmount = decimal.Parse(InvestAmountTextBox.Text);
                TimeFrameListBox.Items.Clear();
                ClearButton.Enabled = true;
                SearchTrxNoGroupBox.Visible = false;
                DisplayButton.Enabled = false;
                InvestAmountTextBox.Enabled = false;
                SummaryButton.Enabled = false;
                TimeFrameListBox.Visible = true;
                FirstProceedButton.Visible = true;
                FirstProceedButton.Enabled = false;
                SummaryGroupBox.Visible = false;
                //Checks the condition and calls the meathod to calculate compound interest(C.I).
                if (PrincipalAmount <= 25000)
                {
                    EndAmount1 = CalculateInterest(PrincipalAmount, LINTERESTRATE1, TIMEFRAME1);
                    EndAmount2 = CalculateInterest(PrincipalAmount, LINTERESTRATE2, TIMEFRAME2);
                    EndAmount3 = CalculateInterest(PrincipalAmount, LINTERESTRATE3, TIMEFRAME3);
                    EndAmount4 = CalculateInterest(PrincipalAmount, LINTERESTRATE4, TIMEFRAME4);
                    //Stores if the listbox along with formatting.
                    TimeFrameListBox.Items.Add(TIMEFRAME1 + "\t" + LINTERESTRATE1.ToString("C3") + "%" + "\t" + EndAmount1.ToString("C4"));
                    TimeFrameListBox.Items.Add(TIMEFRAME2 + "\t" + LINTERESTRATE2.ToString("C3") + "%" + "\t" + EndAmount2.ToString("C4"));
                    TimeFrameListBox.Items.Add(TIMEFRAME3 + "\t" + LINTERESTRATE3.ToString("C3") + "%" + "\t" + EndAmount3.ToString("C4"));
                    TimeFrameListBox.Items.Add(TIMEFRAME4 + "\t" + LINTERESTRATE4.ToString("C3") + "%" + "\t" + EndAmount4.ToString("C4"));
                }
                //Checks the condition and calls the meathod to calculate compound interest(C.I).
                else if (PrincipalAmount>25000)
                {

                    EndAmount1 = CalculateInterest(PrincipalAmount, GINTERESTRATE1, TIMEFRAME1);
                    EndAmount2 = CalculateInterest(PrincipalAmount, GINTERESTRATE2, TIMEFRAME2);
                    EndAmount3 = CalculateInterest(PrincipalAmount, GINTERESTRATE3, TIMEFRAME3);
                    EndAmount4 = CalculateInterest(PrincipalAmount, GINTERESTRATE4, TIMEFRAME4);
                    TimeFrameListBox.Items.Clear();
                    //Stores if the listbox along with formatting.
                    TimeFrameListBox.Items.Add(TIMEFRAME1 + "\t" + GINTERESTRATE1.ToString("C3") + "%" + "\t" + EndAmount1.ToString("C4"));
                    TimeFrameListBox.Items.Add(TIMEFRAME2 + "\t" + GINTERESTRATE2.ToString("C3") + "%" + "\t" + EndAmount2.ToString("C4"));
                    TimeFrameListBox.Items.Add(TIMEFRAME3 + "\t" + GINTERESTRATE3.ToString("C3") + "%" + "\t" + EndAmount3.ToString("C4"));
                    TimeFrameListBox.Items.Add(TIMEFRAME4 + "\t" + GINTERESTRATE4.ToString("C3") + "%" + "\t" + EndAmount4.ToString("C4"));
                }
            }
            //Throws exception if value is not numerical.
            catch
            {
                MessageBox.Show("Input Numerical Value ", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InvestAmountTextBox.SelectAll();
                InvestAmountTextBox.Focus();
            }
        }
    }
}
