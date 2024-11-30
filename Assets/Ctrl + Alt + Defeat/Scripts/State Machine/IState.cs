namespace CAD
{
    public interface IState
    {
        void OnStateEnter(SmartTank tankAI);
        void OnStateUpdate(SmartTank tankAI);
        void OnStateExit(SmartTank tankAI);
    }
}
