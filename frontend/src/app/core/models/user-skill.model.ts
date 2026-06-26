export interface UserSkill {
    id: string;
    userId: string;
    userFullName: string;
    skillId: string;
    skillName: string;
    status: number;
    isTeachable: boolean;
    notes?: string
}

export interface CreateUserSkill {
    userId: string;
    skillId: string;
    status: number;
    isTeachable: boolean;
    notes?: string
}

export interface UpdateUserSkill {
    status?: number;
    isTeachable?: boolean;
    notes?: string;
}
