# EXAMPLE PROMPTS

#generate intro text of selling yellow paint color with max 20 words 

#summarize from field ::Main body::

#summarize in a selling way ::this::

#summarize in a selling way ::6:: => (alt. use ::pageid:6:: or ::pageid:6:main body:: or ::blockid/content/contentid/pageid::)

#give me a good SEO page description about NASA

#translate into norwegian: ::excerpt::

#suggest a 20 words SEO meta description for ::this::

#suggest a 10 words selling SEO meta title for ::main body::

#what is important to think of when you write a SEO Page description?

#what does SEO mean?

#rewrite this text for a 6 year old: "..."

#rewrite this text for a 6 year old: ::excerpt::

#generate lorem ipsum

#generate a cookie consent message

#explain exchange rates for a 6 year old

#what decides the exchange rates

#explain internet for old people

#translate into Spanish: "..."


## MOOD EXAMPLE:

#Rewrite this in more strict tone "..."

#Rewrite this in more provocate tone "..."

#Rewrite this in more serious tone "..."

#Rewrite this in more formal tone "..."

#Rewrite this in more informal tone "..."

#Rewrite this in more humorous tone "..."

#I will type keywords via comma and you will reply with one funny title. my first keywords are paint,house,remodel





## USE PROPERTIES

#combine ::pagename:: with ::Author::

### INTERACT WITH CHATGPT

#based on ::main body:: what should I add?

#based on ::main body:: what is the most impressive?

#based on ::main body:: what is the most negative?

#based on ::this:: what should i add?







## PRODUCT / COMMERCE ( quicksilver solution or Foundation )

#suggest a short SEO title based on ::Display name:: when it is winter

#Summarize with 255 characters this SEO description for brand ::brand::, texts: ::Long description:: and ::Product teaser:: 

#give me popular SEO keywords in coma separated list for ::Long description:: ::Product teaser:: 

#generate SEO keywords in coma separated list from content in :: this::

#extract keywords: SEO stands for search engine optimization, which is a set of practices designed to improve the appearance and positioning of web pages in organic search results. Because organic search is the most prominent way for people to discover and access online content, a good SEO strategy is essential for improving the quality and quantity of traffic to your website.

#check spelling: SEO stands for search engine optimization, which is a set of practices designed to improve the appearance and positioning of web pages in organic search results. Because organic search is the most prominent way for people to discover and access online content, a good SEO strategy is essential for improving the quality and quantity of traffic to your website.


#summarize in a selling way ::productcode:P-654987:: => this needs implementation of Epicweb.Optimizely.AIAssistant.IPlaceholderResolver as of version 1.0.0.3








## IN TINYMCE Rich Text Editor: (SELECT THE PROMPT AND CLICK "AI"-button)

#Create realistic photo of a small cute white bichon frise puppy sitting in the green gras in daylight

#create realistic photo of bank district New York in daylight

#create list of top 5 most popular names this year in the USA

#create list of top 5 most popular names this year in the France

#create table of top 5 most popular currencies and the exchange rate against US dollar

#generate information about a fictive global "Alloy Exchange" money transfer service

#generate information about how money transfer work in the big picture and add H2 appropriately

#generate 10 FAQ and answers about how money transfer, add H2 appropriately

#create illustration of global money transfer exchange service without text

#create realistic photo of bank in new york city

#create 500 euro photo realistic on white background with shadow

#write a 20 words summary with conclusion h2 of ::main body::

#Summarize in a selling way with 20 words ::main body::

#Create a swot analysis of popular crypto currencies and what benefits each has

#Create table swot analysis of popular currencies and what benefits each has

#Create table of the days of the week, in english, Swedish and French


## GIVE ASSISTANT INSTRUCTIONS / Per Property

[AIAssistant(AssistantInstructions = "You are a code assistant return ONLY javascript code, without explaination nor script-tag.")]
    public virtual string InlineCode

Example prompt:

#confirm then hide an cookie element with id cookie

Answer: confirm("Are you sure you want to hide the cookie element?") ? document.getElementById("cookie").style.display = "none" : "";


