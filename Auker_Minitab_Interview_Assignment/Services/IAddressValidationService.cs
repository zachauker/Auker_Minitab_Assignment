using Auker_Minitab_Interview_Assignment.Entities;

namespace Auker_Minitab_Interview_Assignment.Services;

public interface IAddressValidationService
{
    Task<bool> IsAddressValidAsync(Address address);
}