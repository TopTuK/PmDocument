﻿using PmHelper.Domain.Models.Users;

namespace PmHelper.Domain.Models.Documents
{
    public interface IUserDocument : IDocument
    {
        IUser User { get; }
    }
}
