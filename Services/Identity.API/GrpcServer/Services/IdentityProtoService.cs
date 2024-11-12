using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Identity.API.GrpcServer.Protos;
using Identity.API.Repositories.Interfaces;
using ILogger = Serilog.ILogger;
namespace Identity.API.GrpcServer.Services
{
    public record GetUserById(int Result);
    public class IdentityProtoService : IdentityGrpcService.IdentityGrpcServiceBase
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger _logger;
        public IdentityProtoService(IUserRepository userRepository,ILogger logger)
        {
            this.userRepository = userRepository;
            this._logger = logger;
        }

        public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            _logger.Information("START---IdentityProtoService---GetUserById");
            var user = await userRepository.GetByIdAsync(request.Id);
            if (user == null)
                return null;
            _logger.Information("END---IdentityProtoService---GetUserById");
            return new GetUserByIdResponse()
            {
                Country = user.Country,
                Fullname = user.Fullname,
                Gender = user.Gender,
                Phone = user.Phone,
                Id = user.Id,
                BirthDate = Timestamp.FromDateTime(user.BirthDate.ToUniversalTime())
            };
        }

        public override async Task<GetUsersByIdsResponse> GetUsersByIds(GetUsersByIdRequest request, ServerCallContext context)
        {
            _logger.Information("START---IdentityProtoService---GetUsersByIds");
            var uniqueIds = request.Ids.Distinct().ToList();

            var response = new GetUsersByIdsResponse();
            foreach(var item in uniqueIds)
            {
                var user = await userRepository.GetByIdAsync(item);
                if (user != null)
                {
                    response.Users.Add(new UserResponse
                    {
                        Country = user.Country,
                        Fullname = user.Fullname,
                        Gender = user.Gender,
                        Phone = user.Phone,
                        Id = user.Id,
                        BirthDate = Timestamp.FromDateTime(user.BirthDate.ToUniversalTime())
                    });
                }
            }
            _logger.Information("END---IdentityProtoService---GetUsersByIds");
            return response;


        }
    }
}
