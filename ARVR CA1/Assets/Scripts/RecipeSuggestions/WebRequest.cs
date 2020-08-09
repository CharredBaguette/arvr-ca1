using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace Recipes.Suggestions
{
    public class WebRequest : MonoBehaviour
    {
        // A class to build a request for recippuppy.com api
        public class RequestBuilder
        {
            private const string Url = "http://www.recipepuppy.com/api/?i=onions,garlic&p=3";
            private HashSet<string> _ingredients = new HashSet<string>();

            public RequestBuilder AddIngredient(string ingredientName)
            {
                _ingredients.Add(ingredientName);
                return this;
            }

            public string Build()
            {
                string url = Url + "?i=";
                foreach (var ingredient in _ingredients)
                {
                    url += string.Concat(ingredient, ',');
                }

                url.TrimEnd(',');
                url += "&p=1"; // page
                return url;
            }
        }

        /// <summary>
        /// This is event is fired when a request is completed
        /// </summary>
        public OnDataLoadedUnityEvent OnDataLoaded;

        // Start connection to API for weather request.
        // Invokes OnWeatherDataLoaded when data is received.
        public void StartWebRequestText()
        {
            var request = new RequestBuilder();
            string url = request.AddIngredient("onions").AddIngredient("garlic").Build();

            StartCoroutine(GetRequest(url));
        }

        /// <summary>
        /// Start a suggestion request with ingredients as input
        /// </summary>
        public void StartSuggestionRequest(IEnumerable<string> ingredients)
        {
            var request = new RequestBuilder();

            foreach (var ingredient in ingredients)
            {
                _ = request.AddIngredient(ingredient);
            }

            StartCoroutine(GetRequest(request.Build()));
        }

        private IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    ParseJsonFile(webRequest.downloadHandler.text);
                }
            }
        }

        private void ParseJsonFile(string jsonData)
        {
            Debug.Log(jsonData);

            JSONObject queriedJson = (JSONObject)JSON.Parse(jsonData);

            const int MaxSuggestions = 10;
            List<RecipeSuggestion> suggestions = new List<RecipeSuggestion>(MaxSuggestions);

            for (int i = 0; i < MaxSuggestions; ++i)
            {
                var jsonEntry = queriedJson["results"][i];

                string title = jsonEntry["title"];
                title = title.Replace("\n", "").Replace("\r", "").Replace("\t", ""); // Clean newlines and tabs

                string href = jsonEntry["href"];

                string ingredients = jsonEntry["ingredients"];
                string[] ingredientList = ingredients.Split(new string[] { ", " }, StringSplitOptions.None);

                suggestions.Add(new RecipeSuggestion(title, href, ingredientList));
            }

            foreach (var s in suggestions)
            {
                Debug.Log(s.Title + "\n" + s.Href + "\n" + s.Ingredients);
            }

            if (OnDataLoaded != null)
            {
                OnDataLoaded.Invoke(suggestions);
            }
        }

        /// <summary>
        /// A suggestion entry for a recipe
        /// </summary>
        public class RecipeSuggestion
        {
            public string Title { get; private set; }
            public string Href { get; private set; }
            public List<string> Ingredients { get; private set; }
            public RecipeSuggestion(string title, string href, IEnumerable<string> ingredients)
            {
                Title = title;
                Href = href;
                Ingredients = new List<string>(ingredients);
            }
        }

        [Serializable]
        public class OnDataLoadedUnityEvent : UnityEvent<IEnumerable<RecipeSuggestion>> { }
    }
}