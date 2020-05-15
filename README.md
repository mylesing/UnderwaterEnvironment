# Underwater Environment

**Procedural Underwater Environment:**
- **L-System plant generator:** separately created coral and algae l-systems which spawn randomly sized natural-looking vegetation throughout the scene
- **Fish schooling behavior:** fish swim about the scene towards internal targets, and swim towards one another when nearby.
- **Procedural sandscape:** takes in a flat plane and used FBM noise to produce a sandscape. Includes internally computer normals, self-shadowing, and fog.

Corals Inspired by:
http://pcg.fdg2015.org/papers/a_constructive_approach_for_the_generation_of_underwater_environments.pdf 

Fish Schooling Inspired by Boids algorithm:
https://cs.stanford.edu/people/eroberts/courses/soco/projects/2008-09/modeling-natural-systems/boids.html

FBM function:
https://forum.unity.com/threads/mathf-perlinnoise-x-y-in-a-shader.532963/


