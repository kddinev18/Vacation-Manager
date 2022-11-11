namespace ServiceLayer
{
    enum UserOperation
    {
        Register = 0,
        LogIn = 2,
        LogInWithCookies = 3,
        RegisterMember = 4,
        GetUsers = 5,
        GetUserCount = 6,
        RemoveUser = 7,
        EditUser = 8,
        CheckAuthentication = 9,
        AddTeam = 10,
        AddProject = 11,
        GetProjects = 12,
        GetProjectCount = 13,
        EditProject = 14,
        RemoveProject = 15,
        GetTeams = 16,
        GetTeamCount = 17,
        EditTeam = 18,
        RemoveTeam = 19,

        AddVacation = 20,
        GetVacations = 21,
        GetVacationsCount = 22,
        ApproveVacation = 23,

        GetUserByName = 24,
        GetProjectByName = 25,
        GetTeamByName = 26,

        GetCurrentUserInformation = 27,
    }
}
