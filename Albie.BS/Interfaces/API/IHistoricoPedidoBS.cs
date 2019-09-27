using ActioBP.General.Models;
using Albie.Models;

namespace Albie.BS.Interfaces
{
    public interface IHistoricoPedidoBS : IEntityAlbieBS<HistoricoPedido, int>
    {
        HistoricoPedido GetHistoricoFromOrder(Document order);
        ResultAndError<HistoricoPedido> CloseOrder(Document order);
    }
}