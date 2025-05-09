export interface iQualificationInfo {
    code: string;
    description: string;
    points: number;
    level: string;
    courses: iCourseInfo[];
}
export interface iCourseInfo {
    id: number;
    code: string;
    description: string;
    points: number;
}