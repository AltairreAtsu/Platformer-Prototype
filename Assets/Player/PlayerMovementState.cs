public interface PlayerMovementState
{
	void EnterState();
	void Update(float horizontalThrow, float verticalThrow, bool jump, bool dash, bool glide);
}
