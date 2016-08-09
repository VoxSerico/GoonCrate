using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoonCrates
{
    public partial class Form1 : Form
    {
        bool skipEvaluation = false;
        WordSize currentWordSize = WordSize.NINE;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonWordLengthFive_Click(object sender, EventArgs e)
        {
            currentWordSize = WordSize.FIVE;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;
            textBox7.Visible = true;
            textBox8.Visible = false;
            textBox9.Visible = false;
            ResetForm();
        }

        private void buttonWordLengthSeven_Click(object sender, EventArgs e)
        {
            currentWordSize = WordSize.SEVEN;
            textBox1.Visible = false;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;
            textBox7.Visible = true;
            textBox8.Visible = true;
            textBox9.Visible = false;
            ResetForm();
        }

        private void Evaluate()
        {
            char?[] knownLetters;
            switch (currentWordSize)
            {
                case WordSize.FIVE:
                    knownLetters = new char?[5];
                    knownLetters[0] = GetCharOrNull(textBox3);
                    knownLetters[1] = GetCharOrNull(textBox4);
                    knownLetters[2] = GetCharOrNull(textBox5);
                    knownLetters[3] = GetCharOrNull(textBox6);
                    knownLetters[4] = GetCharOrNull(textBox7);
                    break;
                case WordSize.SEVEN:
                    knownLetters = new char?[7];
                    knownLetters[0] = GetCharOrNull(textBox2);
                    knownLetters[1] = GetCharOrNull(textBox3);
                    knownLetters[2] = GetCharOrNull(textBox4);
                    knownLetters[3] = GetCharOrNull(textBox5);
                    knownLetters[4] = GetCharOrNull(textBox6);
                    knownLetters[5] = GetCharOrNull(textBox7);
                    knownLetters[6] = GetCharOrNull(textBox8);
                    break;
                case WordSize.NINE:
                    knownLetters = new char?[9];
                    knownLetters[0] = GetCharOrNull(textBox1);
                    knownLetters[1] = GetCharOrNull(textBox2);
                    knownLetters[2] = GetCharOrNull(textBox3);
                    knownLetters[3] = GetCharOrNull(textBox4);
                    knownLetters[4] = GetCharOrNull(textBox5);
                    knownLetters[5] = GetCharOrNull(textBox6);
                    knownLetters[6] = GetCharOrNull(textBox7);
                    knownLetters[7] = GetCharOrNull(textBox8);
                    knownLetters[8] = GetCharOrNull(textBox9);
                    break;
                default:
                    knownLetters = new char?[0];
                    break;
            }

            List<char> excludedLetters = new List<char>();
            if (checkBoxA.Checked) excludedLetters.Add('A');
            if (checkBoxB.Checked) excludedLetters.Add('B');
            if (checkBoxC.Checked) excludedLetters.Add('C');
            if (checkBoxD.Checked) excludedLetters.Add('D');
            if (checkBoxE.Checked) excludedLetters.Add('E');
            if (checkBoxF.Checked) excludedLetters.Add('F');
            if (checkBoxG.Checked) excludedLetters.Add('G');
            if (checkBoxH.Checked) excludedLetters.Add('H');
            if (checkBoxI.Checked) excludedLetters.Add('I');
            if (checkBoxJ.Checked) excludedLetters.Add('J');
            if (checkBoxK.Checked) excludedLetters.Add('K');
            if (checkBoxL.Checked) excludedLetters.Add('L');
            if (checkBoxM.Checked) excludedLetters.Add('M');
            if (checkBoxN.Checked) excludedLetters.Add('N');
            if (checkBoxO.Checked) excludedLetters.Add('O');
            if (checkBoxP.Checked) excludedLetters.Add('P');
            if (checkBoxQ.Checked) excludedLetters.Add('Q');
            if (checkBoxR.Checked) excludedLetters.Add('R');
            if (checkBoxS.Checked) excludedLetters.Add('S');
            if (checkBoxT.Checked) excludedLetters.Add('T');
            if (checkBoxU.Checked) excludedLetters.Add('U');
            if (checkBoxV.Checked) excludedLetters.Add('V');
            if (checkBoxW.Checked) excludedLetters.Add('W');
            if (checkBoxX.Checked) excludedLetters.Add('X');
            if (checkBoxY.Checked) excludedLetters.Add('Y');
            if (checkBoxZ.Checked) excludedLetters.Add('Z');

            char[] excludeLettersArray = excludedLetters.ToArray();

            List<string> words = Program.GetPossibleWords(currentWordSize, knownLetters, excludeLettersArray);
            labelPossibleWords.Text = string.Join(" ", words);

            char suggestedLetter = Program.GetSuggestedLetter(currentWordSize, knownLetters, excludeLettersArray);
            labelNextGuess.Text = suggestedLetter.ToString();

            excludedLetters.Add(suggestedLetter);
            char suggestedLetter2 = Program.GetSuggestedLetter(currentWordSize, knownLetters, excludedLetters.ToArray());

            excludedLetters.Add(suggestedLetter2);
            char suggestedLetter3 = Program.GetSuggestedLetter(currentWordSize, knownLetters, excludedLetters.ToArray());

            excludedLetters.Add(suggestedLetter3);
            char suggestedLetter4 = Program.GetSuggestedLetter(currentWordSize, knownLetters, excludedLetters.ToArray());

            excludedLetters.Add(suggestedLetter4);
            char suggestedLetter5 = Program.GetSuggestedLetter(currentWordSize, knownLetters, excludedLetters.ToArray());

            excludedLetters.Add(suggestedLetter5);
            char suggestedLetter6 = Program.GetSuggestedLetter(currentWordSize, knownLetters, excludedLetters.ToArray());

            labelNextGuess2.Text = suggestedLetter2.ToString();
            labelNextGuess3.Text = suggestedLetter3.ToString();
            labelNextGuess4.Text = suggestedLetter4.ToString();
            labelNextGuess5.Text = suggestedLetter5.ToString();
            labelNextGuess6.Text = suggestedLetter6.ToString();
        }

        private char? GetCharOrNull(TextBox textBox)
        {
            return string.IsNullOrEmpty(textBox.Text) ? (char?)null : textBox.Text[0];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void buttonWordLengthNine_Click(object sender, EventArgs e)
        {
            currentWordSize = WordSize.NINE;
            textBox1.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            textBox6.Visible = true;
            textBox7.Visible = true;
            textBox8.Visible = true;
            textBox9.Visible = true;
            ResetForm();
        }

        private void ResetForm()
        {
            skipEvaluation = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";

            labelNextGuess.Text = "";
            labelNextGuess2.Text = "";
            labelNextGuess3.Text = "";
            labelNextGuess4.Text = "";
            labelNextGuess5.Text = "";
            labelNextGuess6.Text = "";

            checkBoxA.Checked = false;
            checkBoxB.Checked = false;
            checkBoxC.Checked = false;
            checkBoxD.Checked = false;
            checkBoxE.Checked = false;
            checkBoxF.Checked = false;
            checkBoxG.Checked = false;
            checkBoxH.Checked = false;
            checkBoxI.Checked = false;
            checkBoxJ.Checked = false;
            checkBoxK.Checked = false;
            checkBoxL.Checked = false;
            checkBoxM.Checked = false;
            checkBoxN.Checked = false;
            checkBoxO.Checked = false;
            checkBoxP.Checked = false;
            checkBoxQ.Checked = false;
            checkBoxR.Checked = false;
            checkBoxS.Checked = false;
            checkBoxT.Checked = false;
            checkBoxU.Checked = false;
            checkBoxV.Checked = false;
            checkBoxW.Checked = false;
            checkBoxX.Checked = false;
            checkBoxY.Checked = false;
            checkBoxZ.Checked = false;

            checkBoxA.Enabled = true;
            checkBoxB.Enabled = true;
            checkBoxC.Enabled = true;
            checkBoxD.Enabled = true;
            checkBoxE.Enabled = true;
            checkBoxF.Enabled = true;
            checkBoxG.Enabled = true;
            checkBoxH.Enabled = true;
            checkBoxI.Enabled = true;
            checkBoxJ.Enabled = true;
            checkBoxK.Enabled = true;
            checkBoxL.Enabled = true;
            checkBoxM.Enabled = true;
            checkBoxN.Enabled = true;
            checkBoxO.Enabled = true;
            checkBoxP.Enabled = true;
            checkBoxQ.Enabled = true;
            checkBoxR.Enabled = true;
            checkBoxS.Enabled = true;
            checkBoxT.Enabled = true;
            checkBoxU.Enabled = true;
            checkBoxV.Enabled = true;
            checkBoxW.Enabled = true;
            checkBoxX.Enabled = true;
            checkBoxY.Enabled = true;
            checkBoxZ.Enabled = true;


            skipEvaluation = false;
            Evaluate();
        }

        private void Evaluate_checkbox(object sender, EventArgs e)
        {
            if (skipEvaluation)
            {
                return;
            }
            Evaluate();
        }

        private void Evaluate_textbox(object sender, EventArgs e)
        {
            if (skipEvaluation)
            {
                return;
            }

            char?[] knownLetters = new char?[]{
                GetCharOrNull(textBox1)
                ,GetCharOrNull(textBox2)
                ,GetCharOrNull(textBox3)
                ,GetCharOrNull(textBox4)
                ,GetCharOrNull(textBox5)
                ,GetCharOrNull(textBox6)
                ,GetCharOrNull(textBox7)
                ,GetCharOrNull(textBox8)
                ,GetCharOrNull(textBox9)
            };

            var distinctKnownLetters = knownLetters.Distinct();

            checkBoxA.Enabled = !distinctKnownLetters.Contains('A');
            checkBoxB.Enabled = !distinctKnownLetters.Contains('B');
            checkBoxC.Enabled = !distinctKnownLetters.Contains('C');
            checkBoxD.Enabled = !distinctKnownLetters.Contains('D');
            checkBoxE.Enabled = !distinctKnownLetters.Contains('E');
            checkBoxF.Enabled = !distinctKnownLetters.Contains('F');
            checkBoxG.Enabled = !distinctKnownLetters.Contains('G');
            checkBoxH.Enabled = !distinctKnownLetters.Contains('H');
            checkBoxI.Enabled = !distinctKnownLetters.Contains('I');
            checkBoxJ.Enabled = !distinctKnownLetters.Contains('J');
            checkBoxK.Enabled = !distinctKnownLetters.Contains('K');
            checkBoxL.Enabled = !distinctKnownLetters.Contains('L');
            checkBoxM.Enabled = !distinctKnownLetters.Contains('M');
            checkBoxN.Enabled = !distinctKnownLetters.Contains('N');
            checkBoxO.Enabled = !distinctKnownLetters.Contains('O');
            checkBoxP.Enabled = !distinctKnownLetters.Contains('P');
            checkBoxQ.Enabled = !distinctKnownLetters.Contains('Q');
            checkBoxR.Enabled = !distinctKnownLetters.Contains('R');
            checkBoxS.Enabled = !distinctKnownLetters.Contains('S');
            checkBoxT.Enabled = !distinctKnownLetters.Contains('T');
            checkBoxU.Enabled = !distinctKnownLetters.Contains('U');
            checkBoxV.Enabled = !distinctKnownLetters.Contains('V');
            checkBoxW.Enabled = !distinctKnownLetters.Contains('W');
            checkBoxX.Enabled = !distinctKnownLetters.Contains('X');
            checkBoxY.Enabled = !distinctKnownLetters.Contains('Y');
            checkBoxZ.Enabled = !distinctKnownLetters.Contains('Z');

            Evaluate();
        }
    }
}
