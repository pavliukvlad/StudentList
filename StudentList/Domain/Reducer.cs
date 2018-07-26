namespace StudentList.Domain
{
    public delegate TState Reducer<TState>(TState previousState, object action);
}
