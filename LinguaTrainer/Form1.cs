using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading; // Thread. Sleep
using System.Media;

namespace LinguaTrainer
{
    public partial class Form1 : Form
    {
        SortedList<string, string> wordsList;
        Dictionary<string, string> wordWavList;
        string fileName, wavFileName;
        string currentWavLocation;
        string correctAnswer;
        string italianWord, englishWord;
        string italianParole, englishParole;
        int totalCorrectAnswers, totalLivesLeft, totalLives, totalQuestions, currentQuestion;
        int typeOfTest; // 1 = English to Italian, 2 = Italian to English
        bool settingsShowing;
            // englishWordToItalian, textTestItalianToEnglish, textTestEnglishToItalian, audioTestItalianToEnglish, audioTestVerbatim;

        public Form1()
        {
            InitializeComponent();
            fileName = "wordsList";
            wavFileName = "wavFileList";
            wordsList = new SortedList<string, string>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // load words 
            wordsList = FileHandling.loadWords(fileName);
            // load words/wav list (links words to their audio file) 
            wordWavList = FileHandling.loadWordsWavList(wavFileName);

            italianWord = "ERRORexplosion.wav";
            englishWord = "ERRORexplosion.wav";
            italianParole = "";
            englishParole = "";

            currentWavLocation = "";

            // disable all buttons except for the START button
            btnChechAnswer.Text = "";
            btnChechAnswer.Enabled = false;

            totalLives = 3; // default of 3
            totalQuestions = 20; // default of 20

            // hide hearts 4 to 6 (lives)
            pbHeart4.Visible = false;
            pbHeart5.Visible = false;
            pbHeart6.Visible = false;

            // Hide the image test controls
            pbImageTest01.Visible = false;
            pbImageTest02.Visible = false;
            pbImageTest03.Visible = false;
            pbImageTest04.Visible = false;
            rbImageTest01.Visible = false;
            rbImageTest02.Visible = false;
            rbImageTest03.Visible = false;
            rbImageTest04.Visible = false;

            // enable the speaker
            btnReplayAudio.Enabled = false;

            settingsShowing = false;
        //    englishWordToItalian = true;
        //    textTestItalianToEnglish = true;
        //    textTestEnglishToItalian = true;
        //    audioTestItalianToEnglish = true;
        //    audioTestVerbatim = true;
        }



        /*
         * This method starts the test
         */
        private void btnStartTest_Click(object sender, EventArgs e)
        {
            // make sure at least one type of test has been selected
            if (!cbItalianToEnglish.Checked && !cbEnglishToItalian.Checked && !cbAudioItalianToEnglish.Checked && !cbAudioVerbatim.Checked)
            {
                MessageBox.Show("ERROR - all tests have been deactivated, please exit program NOW!");
                Application.Exit();
            }

            // enable the speaker
            btnReplayAudio.Enabled = true;

            // set totals
            totalLives = Convert.ToInt32(cmbTotalLives.Text); // default of 3
            currentQuestion = 1;
            totalCorrectAnswers = 0;
            totalLivesLeft = totalLives + 1;
            lblTotalCorrectAnswers.Text = "0";
            // enable all buttons
            btnChechAnswer.Enabled = true;
            // make hearts visible
            initialiseHearts();
            // clear the ticks
            pbTick01.Visible = false;
            pbTick02.Visible = false;
            pbTick03.Visible = false;
            pbTick04.Visible = false;
            pbTick05.Visible = false;
            pbTick06.Visible = false;
            pbTick07.Visible = false;
            pbTick08.Visible = false;
            pbTick09.Visible = false;
            pbTick10.Visible = false;
            pbTick11.Visible = false;
            pbTick12.Visible = false;
            pbTick13.Visible = false;
            pbTick14.Visible = false;
            pbTick15.Visible = false;
            pbTick16.Visible = false;
            pbTick17.Visible = false;
            pbTick18.Visible = false;
            pbTick19.Visible = false;
            pbTick20.Visible = false;
            // 
            nextQuestion();

        }

        /*
         * This method sets everything up for a new question
         */
        private void nextQuestion()
        {
            // make sure all lives are still intact
            if (totalLivesLeft >= 1)
            {
                // clear textboxes
                tbAnswerEntryBox.Text = "";
                tbOutputArea.Text = "";
                // set button text to "Check answer"
                btnChechAnswer.Text = "Check answer";
                // choose type of test for the next question
                chooseTypeOfTest();
            }
            else
            {
                allLivesLost();
            }
        }

        /*
         * This method chooses the type of test to carry out:
         * 1) Supply an Italian word or phrase and wait for the player to enter the English translation
         * 2) Supply an English word or phrase and wait for the player to enter the Italian translation
         */
        private void chooseTypeOfTest()
        {
            Random randomNumber = new Random();
            bool getRandomNumber = true;
            while (getRandomNumber)
            {
                typeOfTest = randomNumber.Next(1, 6);
                if (typeOfTest == 1 && cbItalianToEnglish.Checked)
                    getRandomNumber = false; // 
                else if ((typeOfTest == 2 || typeOfTest == 3) && cbEnglishToItalian.Checked)
                    getRandomNumber = false; // 
                else if ((typeOfTest == 4) && cbAudioItalianToEnglish.Checked)
                    getRandomNumber = false; // 
                else if (typeOfTest == 5 && cbAudioVerbatim.Checked)
                    getRandomNumber = false;
            }

            switch (typeOfTest)
            {
                case 1:
                    {
                        // Translate Italian to English
                        italianWordToEnglishTest();
                        break;
                    }
                case 2:
                    {
                        // Translate English to Italian
                        englishWordToItalianTest();
                        break;
                    }
                case 3:
                    {
                        // Translate English to Italian
                        englishWordToItalianTest();
                        break;
                    }
                case 4:
                    {
                        // Audio test 1 - Translate what is said
                        audioTest1();
                        break;
                    }
                default:
                    {
                        // Audio test 2 - Write what is said (verbatim)
                        audioTest2();
                        break;
                    }
            }
        } // END OF chooseTypeOfTest()

        private void italianWordToEnglishTest()
        {
            // choose word
            correctAnswer = chooseItalianWordToDisplay();
        }

        private void englishWordToItalianTest()
        {
            // choose word
            correctAnswer = chooseEnglishWordToDisplay();
        }

        private void audioTest1()
        {
            // choose word
            correctAnswer = chooseItalianWordToSayForTest1();
        }

        private void audioTest2()
        {
            // choose word
            correctAnswer = chooseItalianWordToSayForTest2();
        }

        /*
         * This method randomly picks a word to say and the player must write what they hear (English translation)
         * ITALIAN to ENGLISH
         */
        private string chooseItalianWordToSayForTest1()
        {
            tbTranslateThis.Text = "English translation, please";

            correctAnswer = "";

            Random randomWordIndex = new Random();

            // choose a word
            int totalNumberOfWords = wordsList.Count;

            int wordIdx = randomWordIndex.Next(0, totalNumberOfWords);

            italianParole = wordsList.Keys.ToString();
            englishParole = wordsList.Values.ToString();

            // grab italian word
            string[] italianWords = wordsList.Keys[wordIdx].Split('|');
            italianWord = italianWords[0];
            englishWord = wordsList.Values[wordIdx];

            italianParole = wordsList.Keys[wordIdx];
            englishParole = wordsList.Values[wordIdx];

            // AUDIO
            playAudio(italianWord);

            // return the correct answer (Engliah translation)
            return wordsList.Values[wordIdx];
        }

        /*
         * This method randomly picks a word to say and the player must write what they hear (Italian, not Engliah)
         * ITALIAN to ENGLISH
         */
        private string chooseItalianWordToSayForTest2()
        {
            tbTranslateThis.Text = "What is she saying?";

            correctAnswer = "";

            Random randomWordIndex = new Random();

            // choose a word
            int totalNumberOfWords = wordsList.Count;

            int wordIdx = randomWordIndex.Next(0, totalNumberOfWords);

            // grab italian word
            string[] italianWords = wordsList.Keys[wordIdx].Split('|');
            italianWord = italianWords[0];
            englishWord = wordsList.Values[wordIdx];

            italianParole = wordsList.Keys[wordIdx];
            englishParole = wordsList.Values[wordIdx];

            // AUDIO
            playAudio(italianWord);

            // return the correct answer (Italian word that was spoken)
            return wordsList.Keys[wordIdx];
        }

        /*
         * This method randomly picks a word to display and returns the correct transaltion, ready to be compared with whatever is entered by the player.
         * ITALIAN to ENGLISH
         */
        private string chooseItalianWordToDisplay()
        {
            correctAnswer = "";

            Random randomWordIndex = new Random();

            // choose a word
            int totalNumberOfWords = wordsList.Count;

            int wordIdx = randomWordIndex.Next(0, totalNumberOfWords);

            // display query
            string[] italianWords = wordsList.Keys[wordIdx].Split('|');
            tbTranslateThis.Text = italianWords[0];
            italianWord = italianWords[0];
            englishWord = wordsList.Values[wordIdx];

            italianParole = wordsList.Keys[wordIdx];
            englishParole = wordsList.Values[wordIdx];

            // AUDIO
            playAudio(italianWord);

            // return the correct answer
            return wordsList.Values[wordIdx];
        }

        /*
         * This method randomly picks a word to display and returns the correct transaltion, ready to be compared with whatever is entered by the player.
         * ENGLISH to ITALIAN
         */
        private string chooseEnglishWordToDisplay()
        {
            correctAnswer = "";

            Random randomWordIndex = new Random();

            // choose a word
            int totalNumberOfWords = wordsList.Count;

            int wordIdx = randomWordIndex.Next(0, totalNumberOfWords);

            // display query
            string[] words = wordsList.Values[wordIdx].Split('|');
            tbTranslateThis.Text = words[0];

            string[] italianWords = wordsList.Keys[wordIdx].Split('|');
            italianWord = italianWords[0];

            englishWord = wordsList.Values[wordIdx];

            italianParole = wordsList.Keys[wordIdx];
            englishParole = wordsList.Values[wordIdx];

            // return the correct answer
            return wordsList.Keys[wordIdx];
        }


        private void checkAnswerOrNextQuestion()
        {
            // compare the player's answer with the correct answer
            if (String.Compare(btnChechAnswer.Text, "Check answer") == 0)
            {
                // we are checking the player's answer
                if (checkAnswer())
                {
                    // increase total correct answers
                    totalCorrectAnswers++;
                    tbOutputArea.Text = "Yey, that is correct :D\r\n\r\n" + "Italian word(s): " + italianParole + "\r\nEnglish word(s): " + englishParole;
                    lblTotalCorrectAnswers.Text = totalCorrectAnswers.ToString();
                    displayATick();
                    playSound(@"audio\correctAnswer.wav");
                    Thread.Sleep(1000);
                    if (typeOfTest == 2 || typeOfTest == 3) // we need to make sure the Italian word spoken is the same as the one entered (for when there is more than one)
                    {
                        italianWord = tbAnswerEntryBox.Text;
                    }

                    playAudio(italianWord);
                    Thread.Sleep(1000);
                }
                else
                {
                    // output error, and display correct answer
                    tbOutputArea.Text = "Sorry, that is not correct :(\r\n\r\n" + "Italian word(s): " + italianParole + "\r\nEnglish word(s): " + englishParole;
                    // decrease number of lives left
                    totalLivesLeft -= 1;
                    removeALife();
                    playSound("ERRORexplosion.wav");
                    Thread.Sleep(1000);
                    playAudio(italianWord);
                    Thread.Sleep(1000);
                }
                // change button text to Next question
                btnChechAnswer.Text = "Next question";
            }
            else // we wish to have another question
            {
                currentQuestion++;
                // have we ansered all of the questions
                if (currentQuestion <= totalQuestions)
                {
                    nextQuestion();
                }
                else // no more questions to answer, prompt to start again
                    allQuestionsAnswered();
            }
        }

        private void btnChechAnswer_Click(object sender, EventArgs e)
        {
            checkAnswerOrNextQuestion();
        }

        private void tbAnswerEntryBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //enter key is down
                checkAnswerOrNextQuestion();
            }
        }

        /*
         * This is used to check to see if the answer given by the player is correct. 
         */
        private bool checkAnswer()
        {
            string[] correctAnswers = correctAnswer.Split('|');

            for (int i = 0; i < correctAnswers.Length; i++)
            {
                if (String.Compare(tbAnswerEntryBox.Text.ToLower(), correctAnswers[i].ToLower()) == 0)
                {
                    return true;
                }
            }
            return false;
        }












        /*
         * This method is executed once all the questions have been answere
         */
        private void allQuestionsAnswered()
        {
            // disable all buttons except for the START button
            btnChechAnswer.Text = "";
            btnChechAnswer.Enabled = false;
            // inform player that the test is over
            tbOutputArea.Text = "ALL QUESTIONS ANSWERED! \r\n\r\n" + "Press START to have another go :)";
            playSound(@"audio\endOfTest.wav");
        }

        /*
         * This method is executed once all the player's lives have been lost
         */
        private void allLivesLost()
        {
            // disable all buttons except for the START button
            btnChechAnswer.Text = "";
            btnChechAnswer.Enabled = false;
            // inform player that the test is over
            tbOutputArea.Text = "ALL LIVES HAVE BEEN LOST! \r\n\r\n" + "Press START to have another go :)";
            playSound(@"audio\endOfTest.wav");
        }


        private void removeALife()
        {
            if (pbHeart6.Visible)
                pbHeart6.Visible = false;
            else if (pbHeart5.Visible)
                pbHeart5.Visible = false;
            else if (pbHeart4.Visible)
                pbHeart4.Visible = false;
            else if (pbHeart3.Visible)
                pbHeart3.Visible = false;
            else if (pbHeart2.Visible)
                pbHeart2.Visible = false;
            else pbHeart1.Visible = false;
        }

        private void displayATick()
        {
            if (!pbTick01.Visible)
                pbTick01.Visible = true;
            else if (!pbTick02.Visible)
                pbTick02.Visible = true;
            else if (!pbTick03.Visible)
                pbTick03.Visible = true;
            else if (!pbTick04.Visible)
                pbTick04.Visible = true;
            else if (!pbTick05.Visible)
                pbTick05.Visible = true;
            else if (!pbTick06.Visible)
                pbTick06.Visible = true;
            else if (!pbTick07.Visible)
                pbTick07.Visible = true;
            else if (!pbTick08.Visible)
                pbTick08.Visible = true;
            else if (!pbTick09.Visible)
                pbTick09.Visible = true;
            else if (!pbTick10.Visible)
                pbTick10.Visible = true;
            else if (!pbTick11.Visible)
                pbTick11.Visible = true;
            else if (!pbTick12.Visible)
                pbTick12.Visible = true;
            else if (!pbTick13.Visible)
                pbTick13.Visible = true;
            else if (!pbTick14.Visible)
                pbTick14.Visible = true;
            else if (!pbTick15.Visible)
                pbTick15.Visible = true;
            else if (!pbTick16.Visible)
                pbTick16.Visible = true;
            else if (!pbTick17.Visible)
                pbTick17.Visible = true;
            else if (!pbTick18.Visible)
                pbTick18.Visible = true;
            else if (!pbTick19.Visible)
                pbTick19.Visible = true;
            else pbTick20.Visible = true;
        }

        private void cmbTotalQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            totalQuestions = Convert.ToInt32(cmbTotalQuestions.Text);
        }

        private void initialiseHearts()
        {
            // display only the hearts we want
            if (totalLives >= 1)
                pbHeart1.Visible = true;
            else pbHeart1.Visible = false;
            if (totalLives >= 2)
                pbHeart2.Visible = true;
            else pbHeart2.Visible = false;
            if (totalLives >= 3)
                pbHeart3.Visible = true;
            else pbHeart3.Visible = false;
            if (totalLives >= 4)
                pbHeart4.Visible = true;
            else pbHeart4.Visible = false;
            if (totalLives >= 5)
                pbHeart5.Visible = true;
            else pbHeart5.Visible = false;
            if (totalLives >= 6)
                pbHeart6.Visible = true;
            else pbHeart6.Visible = false;
        }

        private void playSound(string soundFile)
        {
            using (var soundPlayer = new SoundPlayer(soundFile))
            {
                try
                {
                    soundPlayer.Play(); // can also use soundPlayer.PlaySync()
                }
                catch
                {
                    MessageBox.Show("Unable to play sound: " + soundFile);
                }
            }
        }

        private void playAudio(string word)
        {
            // locate name in wav/word list
            currentWavLocation = "ERRORexplosion.wav";
            // Use var keyword to enumerate dictionary
            foreach (var pair in wordWavList)
            {
                if ((string.Compare(pair.Key.ToLower(), word.ToLower()) == 0))
                {
                    currentWavLocation = pair.Value;
                    break;
                }
            }

            playSound(@"audio\" + currentWavLocation);
        }


        /*
         * The next 10 events are for the buttons that display the Italian lettering (characters with accents above them)
         */
        private void cmbTotalLives_SelectedIndexChanged(object sender, EventArgs e)
        {
            totalLives = Convert.ToInt32(cmbTotalLives.Text);
            // make the # of heart images reflect the new total
            initialiseHearts();
        }
        private void btnAbackwardAccent_Click(object sender, EventArgs e)
        {
            // àáèéìíòóùú
            // write the à to the answer entry box
            tbAnswerEntryBox.Text += "à";
        }
        private void btnAForwardAccent_Click(object sender, EventArgs e)
        {
            // àáèéìíòóùú
            // write the á to the answer entry box
            tbAnswerEntryBox.Text += "á";
        }
        private void btnEbackwardAccent_Click(object sender, EventArgs e)
        {
            // àáèéìíòóùú
            // write the è to the answer entry box
            tbAnswerEntryBox.Text += "è";
        }
        private void btnEForwardAccent_Click(object sender, EventArgs e)
        {
            // àáèéìíòóùú
            // write the é to the answer entry box
            tbAnswerEntryBox.Text += "é";
        }
        private void btnIbackwardAccent_Click(object sender, EventArgs e)
        {
            // àáèéìíòóùú
            // write the ì to the answer entry box
            tbAnswerEntryBox.Text += "ì";
        }

        private void btnIForwardAccent_Click(object sender, EventArgs e)
        {
            // àáèéìíòóùú
            // write the í to the answer entry box
            tbAnswerEntryBox.Text += "í";
        }
        private void btnObackwardAccent_Click(object sender, EventArgs e)
        {
            // àáèéìíòóùú
            // write the ò to the answer entry box
            tbAnswerEntryBox.Text += "ò";
        }
        private void btnOForwardAccent_Click(object sender, EventArgs e)
        {
            // àáèéìíòóùú
            // write the ó to the answer entry box
            tbAnswerEntryBox.Text += "ó";
        }
        private void btnUbackwardAccent_Click(object sender, EventArgs e)
        {
            // àáèéìíòóùú
            // write the ù to the answer entry box
            tbAnswerEntryBox.Text += "ù";
        }
        private void btnUForwardAccent_Click(object sender, EventArgs e)
        {
            // àáèéìíòóùú
            // write the ú to the answer entry box
            tbAnswerEntryBox.Text += "ú";
        }

        private void btnReplayAudio_Click(object sender, EventArgs e)
        {
            if (currentWavLocation == "")
                currentWavLocation = "ERRORexplosion.wav";
            playSound(@"audio\" + currentWavLocation);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (settingsShowing)
            {
                // hide the settings panel
                this.Width = 854;
                settingsShowing = false;
                btnSettings.Text = ">";
            }
            else
            {
                // show the setting panel
                this.Width = 1202;
                settingsShowing = true;
                btnSettings.Text = "<";
            }

        }



        // *************************************************************************
        // ***************              SETTINGS             ***********************
        // *************************************************************************


        private void cbItalianToEnglish_CheckedChanged(object sender, EventArgs e)
        {
        //   textTestItalianToEnglish = !textTestItalianToEnglish;
        //   MessageBox.Show("textTestItalianToEnglish " + textTestItalianToEnglish);
        }

        private void cbEnglishToItalian_CheckedChanged(object sender, EventArgs e)
        {
        //    englishWordToItalian = !englishWordToItalian;
        //    textTestEnglishToItalian = !textTestEnglishToItalian;
        //    MessageBox.Show("textTestEnglishToItalian " + englishWordToItalian);
        }

        private void cbAudioItalianToEnglish_CheckedChanged(object sender, EventArgs e)
        {
        //    audioTestItalianToEnglish = !audioTestItalianToEnglish;
        //    MessageBox.Show("audioTestItalianToEnglish " + audioTestItalianToEnglish);
        }

        private void cbAudioVerbatim_CheckedChanged(object sender, EventArgs e)
        {
        //    audioTestVerbatim = !audioTestVerbatim;
        //    MessageBox.Show("audioTestVerbatim " + audioTestVerbatim);
        }

        private void comboWordList_Click(object sender, EventArgs e)
        {
            // Load the files names into the combobox
            string[] filePaths = FileHandling.getListOfFiles(@"WordFileLists\");

            // add file names to the combobox
            comboWordList.Items.Clear();
            for (int i = 0; i < filePaths.Length; i++)
                comboWordList.Items.Add(filePaths[i]);
        }

        private void comboAudioList_Click(object sender, EventArgs e)
        {
            // Load the files names into the combobox
            string[] filePaths = FileHandling.getListOfFiles(@"AudioFileLists\");

            // add file names to the combobox
            comboAudioList.Items.Clear();
            for (int i = 0; i < filePaths.Length; i++)
                comboAudioList.Items.Add(filePaths[i]);
        }

        private void comboWordList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // load the words list
            wordsList = FileHandling.loadWords(comboWordList.Text);

            foreach (KeyValuePair<string, string> entry in wordsList)
            {
                // output contents of wordWavList
                tbOutputArea.Text += "\r\n" + entry.Key;

            }
            tbOutputArea.Text += "\r\n=====================\r\n===================\r\n";
        }

        private void comboAudioList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // load words/wav list (links words to their audio file) 
            wordWavList = FileHandling.loadWordsWavList(comboAudioList.Text);

            MessageBox.Show("Total audio files: " + wordWavList.Count);

            foreach (KeyValuePair<string, string> entry in wordWavList)
            {
                // output contents of wordWavList
                tbOutputArea.Text += "\r\n" + entry.Key;

            }
        }



    }
}