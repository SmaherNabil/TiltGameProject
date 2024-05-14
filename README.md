TiltGameProject
The Tilt Game is a complex and challenging puzzle, where the aim is to navigate sliders across a square grid, maneuvering them around fixed obstacles until they reach a target location. This intricate problem-solving exercise incorporates algorithmic strategy, dynamic programming, and efficient memory management.

The game board is presented in the form of an NxN matrix, with distinct characters representing movable sliders, obstacles or empty spaces. A specific square is denoted as the target. The project goal is to construct an efficient algorithm to identify and execute the sequence of movements necessary to position a slider at the target location, while minimizing memory footprint and observing performance bound constraints.

Key Features:

Input file processing, creating game board from grid matrix. Implementation of efficient movement function, forming subsequent board states based upon specified tilt directions. Graph/tree construction to determine optimal path from initial board state to goal state. Analytical processing to deduce the puzzle's solvability. Output file generation detailing movement sequence, puzzle solvability and step-by-step puzzle configurations. Performance Requirements: The program must operate within defined performance bounds, ensuring that the read file processing, board state generation, and puzzle-solving functions are bounded by O(N^2), where N refers to one dimension of the board. Memory management must be optimized to avoid potential memory errors and to facilitate fast duplicate state checks.

Testing: The code implementation will undergo two phases of testing. The Sample Test Phase will focus on validating code correctness with small test board cases. The Comprehensive Test Phase concentrates on efficiency and correctness, challenging the code with large-scale board cases.

Bonuses: For those seeking an extra challenge, points will be awarded for the addition of an accessible user interface, enabling users to replay the solution steps in sequence offering a dynamic and visual representation of the slider movements, across customizable NxN grid configurations.

The Tilt Game project thereby offers a significant opportunity for deep dive into complex algorithm development and problem-solving processes, challenging students to implement a succinct and efficient solution to this exciting puzzle game.
