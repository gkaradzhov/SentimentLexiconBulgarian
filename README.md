# Sentiment Lexicon for Bulgarian

## Description
The lexicon is automaticaly exctracted from reviews from grabo.bg.
Approximately 110 000 reviews were crawled. 
For calculating sentiment score pointwise mutual information was used.
Stemmer was applied on the words in the lexicon.
This is raw data, there are words in cyrilic, latin. No spell correction applied.

## Exctraction
The project used for lexicon extraction is written in C#. 
Current state: quite messy, but does the job

###Credit
You are free to use this lexicon or project however you like, but if you want to give me some credit:
Georgi Karadjov, electronic mail: georgi.m.karadjov[at]gmail[dot]com