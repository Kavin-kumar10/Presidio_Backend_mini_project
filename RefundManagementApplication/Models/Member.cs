﻿using System.ComponentModel.DataAnnotations;
using RefundManagementApplication.Models.Enums;

namespace RefundManagementApplication.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public MemberRole Role { get; set; } = MemberRole.User;
        public Plan Membership { get; set; } = Plan.Free;
        public IList<Order> orders { get; set; }
    }
}


//Role -> Admin,User,Collector
