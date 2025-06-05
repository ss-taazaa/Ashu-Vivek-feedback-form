using System.Threading.Tasks;
using FeedbackForm.DTOs;

namespace FeedbackForm.Services.Interfaces
{
    public interface IResponseService
    {
        Task SubmitFormAsync(SubmitFormRequestDto dto);
    }
}
