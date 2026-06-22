export interface Skill {
    id: string;
    name: string;
    category?: string;
    description?: string;
    learnerCount: number;
}

export interface CreateSkill {
    name: string;
    category ?: string;
    description?: string;
}
