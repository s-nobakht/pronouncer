# Pronouncer
A tool for learning and memorizing the pronunciation and meaning of English words based on cognitivie curve of learning.

# What does this code do?
This project is designed to memorize English words and their meanings in Persian by listening to the English audio file and its Persian equivalent.
The idea of ​​this program is based on the production of audio files containing the English pronunciation of the word and its Persian meaning with permutations and the desired number of repetitions. For this purpose, we must first prepare the list of words in the form of collections of records with three fields for each word containing the word in question, the English audio file of its pronunciation, the audio file of its Persian meaning.
To create this list, a Crawler is designed to download these files, which is available in another git repository. Also, to facilitate the preparation of the audio file of the meanings of the words in Persian, the meanings of the words can be recorded with a short pause between the words in the form of a file. Then, using tools such as [https://github.com/tyiannak/pyAudioAnalysis](https://github.com/tyiannak/pyAudioAnalysis), it automatically segmented the recorded file and converted it into separate files for each word.
Pronouncer can be very useful for students who are preparing for English language tests such as TOEFL or IELTS.

# Example of working with this tool
We want to create an audio collection to learn the famous 504 vocabulary. The steps will be as follows:
1. Select the option related to the words of this collection from the collections section.
2. Select the path of Persian and English pronunciation files.
3. Specify the path and name of the output file.
4. Press the data load button. Collection data is loaded.
5. We can add or remove any word from the list we want to create by selecting it.
6. We can see the Persian-English equivalent of the word and hear the sound of each.
7. In the repetition style section, we can select the desired permutations from English, Persian and silent pronunciation.
8. Finally, by selecting the Generate option, the desired audio file that contains the desired pattern of repeating the pronunciation of words will be produced.

![The Pronouncer UI](/docs/pronouncer-ui.png "The Pronouncer UI")