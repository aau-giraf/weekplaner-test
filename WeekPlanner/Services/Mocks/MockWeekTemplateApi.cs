using System.Collections.Generic;
using System.Threading.Tasks;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace WeekPlanner.Services.Mocks
{
    public class MockWeekTemplateApi : IWeekTemplateApi
    {
        public Configuration Configuration { get; set; }
        public string GetBasePath()
        {
            throw new System.NotImplementedException();
        }

        public ExceptionFactory ExceptionFactory { get; set; }
        public ResponseWeekDTO V1WeekTemplateByIdGet(long? id)
        {
            throw new System.NotImplementedException();
        }

        public ApiResponse<ResponseWeekDTO> V1WeekTemplateByIdGetWithHttpInfo(long? id)
        {
            throw new System.NotImplementedException();
        }

        public ResponseIEnumerableWeekNameDTO V1WeekTemplateGet()
        {
            throw new System.NotImplementedException();
        }

        public ApiResponse<ResponseIEnumerableWeekNameDTO> V1WeekTemplateGetWithHttpInfo()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseWeekDTO> V1WeekTemplateByIdGetAsync(long? id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ApiResponse<ResponseWeekDTO>> V1WeekTemplateByIdGetAsyncWithHttpInfo(long? id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseIEnumerableWeekNameDTO> V1WeekTemplateGetAsync()
        {
            List<WeekNameDTO> dtos = new List<WeekNameDTO>{
                new WeekNameDTO("Louise", 1),
                new WeekNameDTO("Kasper", 2),
                new WeekNameDTO("Teitur", 3)
            };
            return new ResponseIEnumerableWeekNameDTO
            {
                Data = dtos,
                Success = true,
                ErrorKey = ResponseIEnumerableWeekNameDTO.ErrorKeyEnum.NoError
            };
        }

        public async Task<ApiResponse<ResponseIEnumerableWeekNameDTO>> V1WeekTemplateGetAsyncWithHttpInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}