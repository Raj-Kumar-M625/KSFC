using Application.DTOs.Master;
using Application.Interface.Persistence.Master;
using Application.UserStories.Master.Request.Queries;
using AutoMapper;
using Common.ConstantVariables;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Master.Handler.Queries
{
    public class GetDropDownListRequestHandler : IRequestHandler<GetDropDownListRequest, DropDownDto>
    {


        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMapper _mapper;

        public GetDropDownListRequestHandler(ICommonMasterRepository commonMaster, IMapper mapper)
        {
            _commonMaster = commonMaster;
            _mapper = mapper;
        }
        public async Task<DropDownDto> Handle(GetDropDownListRequest request, CancellationToken cancellationToken)
        {
            var dropDowns = await _commonMaster.GetCommoMasterValues();
            var dropDownList= _mapper.Map<List<DropDownDto>>(dropDowns);
            var dropDownDto = new DropDownDto
            {
                CategoryType = dropDownList.Where(x => x.CodeType == ValueMapping.categoryType),
                PaymentMethodType= dropDownList.Where(x => x.CodeType == ValueMapping.paymentMethod),
                PaymentTermsType= dropDownList.Where(x => x.CodeType == ValueMapping.paymentType),
                GSTRegistrationType= dropDownList.Where(x => x.CodeType == ValueMapping.gstRegistration),
                TDSSectionType= dropDownList.Where(x => x.CodeType == ValueMapping.tdsSectionType),
                VendorDocumentType= dropDownList.Where(x => x.CodeType == ValueMapping.docType),
                Cities= dropDownList.Where(x => x.CodeType == ValueMapping.cityType),
                States= dropDownList.Where(x => x.CodeType == ValueMapping.stateType),
                Country = dropDownList.Where(x => x.CodeType == ValueMapping.CountryType),
                BillDocumentType = dropDownList.Where(x => x.CodeType == ValueMapping.billDocType),
                CBF = dropDownList.Where(x => x.CodeType == ValueMapping.CBF),
                LabourWelfareCess = dropDownList.Where(x => x.CodeType == ValueMapping.LabourWelfareCess),


            };
            return dropDownDto;
        }
    }
}