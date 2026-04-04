using ErrorOr;
using MediatR;

namespace ProjectTemplate2024.Application.Account.Queries.GetUserDetails;

public class GetUserDetailsQuery : IRequest<ErrorOr<GetUserDetailsResult>> { }
