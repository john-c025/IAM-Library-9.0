using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.auth;
using IAM_Library.models.geneology;
using IAM_Library.models.reports;
using IAM_Library.reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.geneology
{
    public class GeneologyLoader(AuthApiResponseData credentials, HttpClient _httpClient)
    {
        private static string apiBaseUrl = Encryption.decodeString(_constants.authBaseUrl);

        private GeneologyAPIClient geneologyClient = new GeneologyAPIClient(apiBaseUrl, credentials, _httpClient);

        public async Task<GeneologyMainModel> LoadGeneologyData()
        {
            try
            {
                var geneologyData = await geneologyClient.LoadGeneology();

                return geneologyData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<GeneologyMainModel> LoadGeneologyDataDown(string id)
        {
            string accountKey = Encryption.MD5Hash(id);
            try
            {
                var geneologyData = await geneologyClient.LoadGeneologyDown(accountKey);

                return geneologyData;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<GenealogySummary>> LoadGeneologyHistory()
        {
            try
            {
                var geneologyData = await geneologyClient.GetGenealogyHistory();

                return geneologyData.Data;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
