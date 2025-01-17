using backend.Interfaces.Layers.BLL;

namespace backend.Interfaces.Layers.VL;

public interface IGymVisitValidation
{
    public Task<List<string>> CheckIn(CheckInInputData checkInInputData);
    public Task<List<string>> CheckOut(CheckOutInputData CheckOutInputData);
}