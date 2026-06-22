export interface UserSkill {
    id: string;
    userId: string;
    userFullName: string;
    skillId: string;
    skillName: string;
    status: string;
    isTeachable: boolean;
    notes?: string
}

export interface CreateUserSkill {
    userId: string;
    skillId: string;
    status: string;
    isTeachable: boolean;
    notes?: string
}
