using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface ICrossReferenceBS : IEntityAlbieBS<CrossReference>
    {
        CrossReference Get(string itemNo, string unitOfMeasure, string typeNo, string no);
    }
}