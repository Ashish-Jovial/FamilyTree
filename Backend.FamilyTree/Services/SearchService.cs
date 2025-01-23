using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Clients.Elasticsearch;
using Models.FamilyTree.Models;

namespace Backend.FamilyTree.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResultModel>> SearchAsync(SearchRequestModel request);
    }
    public class SearchService : ISearchService
    {
        private readonly ElasticsearchClient _elasticClient;

        public SearchService(ElasticsearchClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<SearchResultModel>> SearchAsync(SearchRequestModel request)
        {
            var searchRequest = new SearchRequest
            {
                Query = new MultiMatchQuery
                {
                    Query = request.Query,
                    Fields = new[] { "name", "additionalInfo" }
                },
                From = (request.Page - 1) * request.PageSize,
                Size = request.PageSize
            };

            if (!string.IsNullOrEmpty(request.FilterField) && !string.IsNullOrEmpty(request.FilterValue))
            {
                searchRequest.Query = new BoolQuery
                {
                    Must = new List<Query>
                        {
                            new MultiMatchQuery
                            {
                                Query = request.Query,
                                Fields = new[] { "name", "additionalInfo" }
                            }
                        },
                    Filter = new List<Query>
                        {
                            new TermQuery(new Field(request.FilterField))
                            {
                                //Field = new Field(request.FilterField),
                                Value = request.FilterValue
                            }
                        }
                };
            }

            var response = await _elasticClient.SearchAsync<SearchResultModel>(searchRequest);

            return response.Documents;
        }
    }
}
