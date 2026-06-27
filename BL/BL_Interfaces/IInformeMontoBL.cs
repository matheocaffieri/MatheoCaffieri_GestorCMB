using System;
using DomainModel;

namespace BL.BL_Interfaces
{
    public interface IInformeMontoBL
    {
        InformeMonto Recalcular(Guid idProyecto);
    }
}
