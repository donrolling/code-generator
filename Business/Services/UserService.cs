using System;
using System.Collections.Generic;
using Business.Common.Responses;
using Business.Service.Interfaces;
using Data.Models.Entities;
using Data.Repository.Interfaces;

namespace Business.Services {
	public class UserService : IUserService {
		public IMembershipService MembershipService { get; set; }
		public IUserRepository UserRepository { get; set; }

		public UserService(IMembershipService membershipService, IUserRepository userRepository) {
			this.MembershipService = membershipService;
			this.UserRepository = userRepository;
		}

		public TransactionResponse Create(User user, string password, string salt) {
			var result = this.UserRepository.Create(user, password, salt);
			return result;
		}

		public TransactionResponse Update(User user){
			var result = this.UserRepository.Update(user);
			return result;
		}

		public TransactionResponse Delete(long id){
			var result = this.UserRepository.Delete(id, this.MembershipService.CurrentUserId());
			return result;
		}
	}
}