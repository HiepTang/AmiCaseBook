﻿using AmeCaseBookOrg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmeCaseBookOrg.Service
{
    public interface IFileService
    {
        File getFile(int fileId);
    }
}