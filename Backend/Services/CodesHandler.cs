using Backend.DataAccess;
using Backend.DTO.Requests;
using Backend.DTO.Responses;
using Backend.Errors;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Services
{
    public class CodesHandler
    {
        private readonly ApplicationContext _context;

        public CodesHandler(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<CreateCodeResponse> CreateAsync(CreateCodeRequest createCodeRequest, CancellationToken cancellationToken)
        {
            if (await _context.Codes.Where(x => x.SerialNumber == createCodeRequest.SerialNumber).AnyAsync(cancellationToken))
            {
                throw new ApiException($"Code with serial number: {createCodeRequest.SerialNumber} already exists", HttpStatusCode.BadRequest);
            }

            var isUsed = false;

            var newCode = new Code()
            {
                SerialNumber = createCodeRequest.SerialNumber,
                IsUsed = isUsed
            };

            await _context.Codes.AddAsync(newCode, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateCodeResponse(newCode.SerialNumber, isUsed);
        }

        public async Task<CodeListResponse> GetAllCodesAsync(CancellationToken cancellationToken)
        {
            var codesList = await _context.Codes.OrderBy(x => x.CodeId).ToListAsync(cancellationToken);
            return new CodeListResponse(codesList);
        }

        public async Task<UpdateCodeResponse> UpdateCodeAsync(UpdateCodeRequest updateCodeRequest, CancellationToken cancellationToken)
        {
            var codeToUpdate = await _context.Codes.FirstAsync(x => x.CodeId == updateCodeRequest.Id, cancellationToken);

            if (codeToUpdate == null)
            {
                throw new ApiException($"Code with: {updateCodeRequest.Id} id not found ", HttpStatusCode.NotFound);
            }

            codeToUpdate.SerialNumber = updateCodeRequest.SerialNumber ?? codeToUpdate.SerialNumber;
            codeToUpdate.IsUsed = updateCodeRequest.IsUsed;

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateCodeResponse(updateCodeRequest.Id, codeToUpdate.SerialNumber, codeToUpdate.IsUsed);
        }
    }
}
