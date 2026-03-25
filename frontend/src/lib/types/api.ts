export interface ApiResponse<T> {
    data: T | null;
    isSuccess: boolean;
    message: string;
}
