﻿using System;
using System.IO;

namespace Continuous.User.Users
{
    internal class ScriptsBoundle
    {
        private readonly string _currentPath;
        
        public ScriptsBoundle()
        {
            _currentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Users", "Scripts");
        }

        public string CreateUser => Path.Combine(_currentPath, "CreateUser.ps1");
        public string RemoveUser => Path.Combine(_currentPath, "RemoveUser.ps1");
        public string GetUser => Path.Combine(_currentPath, "GetUser.ps1");
    }
}
