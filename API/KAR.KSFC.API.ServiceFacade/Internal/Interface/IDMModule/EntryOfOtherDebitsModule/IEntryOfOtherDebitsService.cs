using AutoMapper;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.EntryOfOtherDebitsModule
{
    // <Summary>
    // Author: Gagana K; Module:EntryOfOtherDebits; Date: 27/10/2022
    // <summary>
    public interface IEntryOfOtherDebitsService
    { 
        Task<IEnumerable<IdmOthdebitsDetailsDTO>> GetAllOtherDebitsList(long accountNumber, CancellationToken token);
        Task<bool> UpdateOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token);
        Task<bool> DeleteOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token);
        Task<bool> CreateOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token);
        Task<bool> SubmitOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token);
    }
}

