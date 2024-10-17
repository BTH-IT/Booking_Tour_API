using Grpc.Core;
using Identity.API.GrpcServer.Protos;
using Identity.API.Repositories.Interfaces;
using System.Net.WebSockets;

namespace Identity.API.GrpcServer.Services
{
    public record GetUserById(int Result);
    public class IdentityProtoService : IdentityGrpcService.IdentityGrpcServiceBase
    {
        private readonly IUserRepository userRepository;
        public IdentityProtoService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
        {
            var user = await userRepository.GetByIdAsync(request.Id);
            if (user == null)
                return null;
            return new GetUserByIdResponse()
            {
                Country = user.Country,
                Fullname = user.Fullname,
                Gender = user.Gender,
                Phone = user.Phone,
                Id = user.Id
            };
        }
    }
}
