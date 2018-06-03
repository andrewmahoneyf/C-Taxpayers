/* Created by: Andrew Mahoney-Fernandes on 5/29/18, Final tax app
 * Inputs: name, investment, exemption, married status
 * Outputs: summary info for all taxpayers as well as individual taxpayers
 * Description: app takes in taxpayer info, creates tow lists for married
 * and unmarried taxpayers and then allows the user to dispay information
 * or save it to a file.
 **/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AMahoneyFernandes_FinalApp
{
    public partial class Form1 : Form
    {
        private List<Taxpayer> MarriedList = new List<Taxpayer>();
        private List<Taxpayer> UnMarriedList = new List<Taxpayer>();
        private int saveTaxpayerCount;

        public Form1()
        {
            InitializeComponent();
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            ClearInputs();
            inputBox.Visible = true; 
        }

        private void ClearInputs()
        {
            marriedCheckBox.Checked = false;

            nameTextBox.Clear();
            salaryTextBox.Clear();
            investmentTextBox.Clear();
            exempTextBox.Clear();

            nameTextBox.Focus();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            Taxpayer taxpayer;
            string name;
            bool married;
            decimal salaryDec, investmentDec;
            int exempCountInt;

            name = nameTextBox.Text;
            married = marriedCheckBox.Checked;
            salaryDec = decimal.Parse(salaryTextBox.Text);
            investmentDec = decimal.Parse(investmentTextBox.Text);
            exempCountInt = int.Parse(exempTextBox.Text);
            taxpayer = new Taxpayer(name, married, salaryDec, investmentDec, exempCountInt);

            if (married) {
                MarriedList.Add(taxpayer);
            } else {
                UnMarriedList.Add(taxpayer);
            }
            displayButton.Focus();
            inputBox.Visible = false;
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            try {
                ReadData();
            } catch (System.IO.FileNotFoundException) {
                Console.WriteLine("System.IO.FileNotFoundException");
            } catch (Exception) {
                Console.WriteLine("Exception");
            }
        }

        private void ReadData()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "All text files|*.txt"
            };
            dialog.ShowDialog();
            string fileName = dialog.FileName;
            dialog.Dispose();
            using (StreamReader reader = new StreamReader(fileName)) {
                while (!reader.EndOfStream) {
                    string[] lineItem;
                    Taxpayer taxpayer;
                    string name;
                    bool married;
                    decimal salaryDec;
                    decimal investmentDec;
                    int exempCountInt;

                    lineItem = reader.ReadLine().Split(new char[] { ',' });
                    name = lineItem[0];
                    married = bool.Parse(lineItem[4]);
                    salaryDec = decimal.Parse(lineItem[1]);
                    investmentDec = decimal.Parse(lineItem[2]);
                    exempCountInt = int.Parse(lineItem[3]);
                    taxpayer = new Taxpayer(name, married, salaryDec, investmentDec, exempCountInt);

                    if (married) {
                        MarriedList.Add(taxpayer);
                    } else {
                        UnMarriedList.Add(taxpayer);
                    }
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (isEmpty()) {
                emptyOutput();
            } else {
                SaveData();
                saveTaxpayerCount = MarriedList.Count + UnMarriedList.Count;
            }
        }

        private void SaveData()
        {
            if (saveTaxpayerCount == (MarriedList.Count + UnMarriedList.Count)) {
                outputTextBox.AppendText("No unsaved data." + Environment.NewLine);
            } else {
                try {
                    SaveFileDialog dialog = new SaveFileDialog
                    {
                        Filter = "All text files|*.txt"
                    };
                    dialog.ShowDialog();
                    string fileName = dialog.FileName;
                    dialog.Dispose();
                    StreamWriter writer = new StreamWriter(fileName, true);
                    string output;
                    foreach (Taxpayer taxpayer in this.MarriedList) {
                        output = taxpayer.Save();
                        writer.WriteLine(output);
                    }
                    foreach (Taxpayer taxpayer2 in this.UnMarriedList) {
                        output = taxpayer2.Save();
                        writer.WriteLine(output);
                    }
                    writer.Close();
                    writer.Dispose();
                    FileInfo info = new FileInfo(fileName);
                    MessageBox.Show("saved to: " + info.Name);
                }
                catch (System.IO.FileNotFoundException) {
                    Console.WriteLine("System.IO.FileNotFoundException");
                } 
                catch (Exception) {
                    Console.WriteLine("Exception");
                }
            }
        }

        private void displayButton_Click(object sender, EventArgs e)
        {
            if (isEmpty()) {
                emptyOutput();
            } else {
                outputTextBox.AppendText("Married Taxpayers:" + Environment.NewLine);
                foreach (Taxpayer taxpayer in this.MarriedList) {
                    outputTextBox.AppendText(taxpayer.Display() + Environment.NewLine);
                }
                outputTextBox.AppendText(System.Environment.NewLine + "Unmarried Taxpayers:" + Environment.NewLine);
                foreach (Taxpayer taxpayer2 in this.UnMarriedList) {
                    outputTextBox.AppendText(taxpayer2.Display() + Environment.NewLine);
                }
            }
        }

        private void summaryButton_Click(object sender, EventArgs e)
        {
            if (isEmpty()) {
                emptyOutput();
            } else {
                outputTextBox.AppendText(Taxpayer.GetSummary() + Environment.NewLine);
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            if (isEmpty()) {
                emptyOutput();
            } else {
                SaveData();
                Taxpayer.ResetData();
                ResetLists();
                outputTextBox.Clear();
                enterButton.Focus();
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            if (!isEmpty()) {
                SaveData();
            } 
            ResetLists();
            Application.Exit();
        }

        private void ResetLists()
        {
            MarriedList.Clear();
            UnMarriedList.Clear();
            saveTaxpayerCount = 0;
        }

        private bool isEmpty()
        {
            if ((MarriedList.Count == 0) && (UnMarriedList.Count == 0)) {
                return true;
            }
            return false;
        }

        private void emptyOutput()
        {
            outputTextBox.Clear();
            outputTextBox.AppendText("No Data Available" + Environment.NewLine);
            enterButton.Focus();
        }

        private void salaryTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // allow decimal numbers
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void investmentTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // allow decimal numbers
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void exempTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
