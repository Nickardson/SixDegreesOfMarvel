using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using HtmlAgilityPack;
using MarvelApi.Tasks.Models;
using MarvelApi.Tasks.Models.CategoryMembers;
using MarvelApi.Tasks.Models.ParsePage;
using Newtonsoft.Json;

namespace MarvelApi.Tasks
{
    public class MarvelApiTasks
    {
        private readonly HttpClient _client;

        public MarvelApiTasks()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://marvel.com/universe3zx/")
            };

            _client.DefaultRequestHeaders.Accept.Clear();
        }

        public string NormalizePageName(string pageName)
        {
            return pageName.Replace('_', ' ');
        }

        /// <summary>
        /// Pulls the group affilations down from the marvel site for the given character.
        /// </summary>
        /// <param name="characterPageName">Name of the character's page. If it is not a character page, the behavior is undefined.</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetGroupAffiliations(string characterPageName)
        {
            characterPageName = NormalizePageName(characterPageName);

            var parsed = await GetPageContents(characterPageName);
            var doc = ParseXmlFromPageModel(parsed);
            var affiliationLinks = GetGroupAffiliationLinksFromCharacterPageContents(doc);
            var affiliationPages = affiliationLinks
                .Select(Path.GetFileName)
                .Select(NormalizePageName);
            
            return affiliationPages;
        }

        public async Task<IEnumerable<string>> GetCharacterAffiliations(string groupPageName)
        {
            groupPageName = NormalizePageName(groupPageName);

            var parsed = await GetPageContents(groupPageName);
            var doc = ParseXmlFromPageModel(parsed);
            var affiliationLinks = GetCharacterAffiliationLinksFromGroupPageContents(doc);
            var affiliationPages = affiliationLinks
                .Select(Path.GetFileName)
                .Select(NormalizePageName);

            return affiliationPages;
        }

        private static HtmlDocument ParseXmlFromPageModel(ParsePageModel parsed)
        {
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(parsed.Parse.Text.Content);
                return doc;
            }
            catch (Exception e)
            {
                throw new MarvelHostException("The page returned by the Marvel Wiki failed to parse. " + parsed, e);
            }
        }

        private static IEnumerable<string> GetGroupAffiliationLinksFromCharacterPageContents(HtmlDocument document)
        {
            return EnumerateLinks(document, "//*[@id = 'char-affiliation-content']//a[not(contains(@class, 'new'))]");
        }

        private static IEnumerable<string> GetCharacterAffiliationLinksFromGroupPageContents(HtmlDocument document)
        {
            return EnumerateLinks(document, "//*[@id = 'currentmembers']//a[not(contains(@class, 'new'))]");
        }

        private static IEnumerable<string> EnumerateLinks(HtmlDocument document, string xpath)
        {
            var nodes = document.DocumentNode.SelectNodes(xpath);

            if (nodes == null)
            {
                yield break;
            }

            foreach (var node in nodes)
            {
                var attr = node.GetAttributeValue("href", null);

                if (attr != null)
                {
                    yield return attr;
                }
            }
        }

        private async Task<ParsePageModel> GetPageContents(string pageName)
        {
            // https://marvel.com/universe3zx/api.php?action=parse&format=json&page=Warlock_(Technarchy)

            var response = await _client.GetAsync($"api.php?action=parse&format=json&page={pageName}");

            if (!response.IsSuccessStatusCode)
            {
                throw new MarvelHostException("The Marvel Wiki returned an error status code of " + response.StatusCode);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var parsedPageModel = JsonConvert.DeserializeObject<ParsePageModel>(responseContent);
            return parsedPageModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryName">i.e. Category:People</param>
        /// <param name="continuePage"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<QueryCategoryMembersModel> GetCategoryMembers(string categoryName, string continuePage = null, int limit = 100)
        {
            // TODO: is normalizing needed here?

            // https://marvel.com/universe3zx/api.php?action=query&format=json&list=categorymembers&cmtitle=Category:People&cmlimit=100
            // https://marvel.com/universe3zx/api.php?action=query&format=json&list=categorymembers&cmtitle=Category:People&cmlimit=100&cmcontinue=Anubis|
            var uri = $"api.php?action=query&format=json&list=categorymembers&cmtitle={categoryName}&cmlimit={limit}";

            if (continuePage != null)
            {
                uri += $"&cmcontinue={continuePage}";
            }

            var response = await _client.GetAsync(uri);
            var responseContent = await response.Content.ReadAsStringAsync();

            var categoryMembersModel = JsonConvert.DeserializeObject<QueryCategoryMembersModel>(responseContent);
            return categoryMembersModel;
        }
    }
}
