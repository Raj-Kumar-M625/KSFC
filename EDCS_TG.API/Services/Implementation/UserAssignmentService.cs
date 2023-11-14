using AutoMapper;
using EDCS_TG.API.Data;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EDCS_TG.API.Services.Implementation
{
    public class UserAssignmentService : IUserAssignmentService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        private readonly KarmaniDbContext _karmaniDbContext;
        private int a;

        public UserAssignmentService(IUnitOfWork repository, KarmaniDbContext karmaniDbContext,IMapper mapper)
        {
            _repository = repository;
            _karmaniDbContext = karmaniDbContext;
            _mapper = mapper;

        }   

        public async Task<UserAssignment> AssignUser(UserAssignment userAssignment)
        {
           var result = await _repository.UserAssignmentREpository.Create(userAssignment);
            return result;
        }

        public async Task<IEnumerable<joinModel>> GetAllAssignedUsers()
        {
           
            var query = (from user in _karmaniDbContext.Set<User>()
                        join AssignedUser in _karmaniDbContext.Set<UserAssignment>()
                            on user.Id equals AssignedUser.UserId
                        select new { user, AssignedUser }).ToList();

            List<joinModel> result = new List<joinModel>();
            foreach(var q in query)
            {
                joinModel joinmodel = new joinModel();
                joinmodel.user = q.user;
                joinmodel.userAssignment = q.AssignedUser;
                result.Add(joinmodel);
            }
             
          
            if (result == null)
            {
                return null;
            }
            return result;

        }

        public Task<IEnumerable<UserAssignment>> getAssignedUserByDistrict(string district)
        {
            var assignedUser = _repository.UserAssignmentREpository.FindByCondition(t => t.District == district);
            return assignedUser;
        }

        public Task<IEnumerable<UserAssignment>> getAssignedUserByHobli(string hobli)
        {
            var assignedUser = _repository.UserAssignmentREpository.FindByCondition(t => t.Hobli == hobli);
            return assignedUser;
        }

        public async Task<UserAssignment> GetAssignedUserById(int id)
        {
            var result = await _repository.UserAssignmentREpository.FindById(t => t.Id == id);

            return result;
        }

        public Task<IEnumerable<UserAssignment>> getAssignedUserByTaluk(string taluk)
        {
            var assignedUser = _repository.UserAssignmentREpository.FindByCondition(t => t.Taluk == taluk);
            return assignedUser;
        }

        public async Task<UserAssignment> RemoveUser(Guid id)
        {
            var result = await _repository.UserAssignmentREpository.FindById(t => t.UserId == id);
            if(result == null)
            {
                return result;
            }

            var entity = await _repository.UserAssignmentREpository.Delete(result);
            return entity;
        }

      
    }
}
