# Game Design Document: "Don't Fall Behind"
## Simple Mountain Guide Game

---

## 1. GAME OVERVIEW

### 1.1 High Concept
A simple physics-based game where you guide 3 tourists up a mountain. If any tourist falls, you restart with the same mountain layout but now you know the dangers.

### 1.2 Genre
- **Primary**: Physics-based Puzzle
- **Platform**: PC (Unity)
- **Target Audience**: All ages

### 1.3 Core Loop Theme Integration
The "Loop" theme is simple:
- **Same mountain layout** - You restart with identical terrain
- **Player learning** - You remember where dangers are
- **Tourist behavior** - Simple AI that follows your path

---

## 2. GAME MECHANICS

### 2.1 Core Gameplay Loop
1. **Start**: Guide 3 tourists up a simple mountain path
2. **Dangers**: Avoid falling rocks, slippery areas, and gaps
3. **Failure**: If any tourist falls, restart with same layout
4. **Success**: Reach the top with all tourists alive

### 2.2 Player Character (Mountain Guide)
- **Movement**: Simple WASD movement
- **Interaction**: Click to grab and drag tourists
- **Equipment**: Just your hands to help tourists

### 2.3 Tourist AI System
- **Simple AI**: Tourists follow your path automatically
- **Physics**: Tourists can fall and get hurt
- **Rescue**: Click to grab fallen tourists

### 2.4 Core Mechanics

#### 2.4.1 Simple Hazards
- **Falling Rocks**: Random rocks fall from above
- **Slippery Areas**: Ice patches that make tourists slide
- **Gaps**: Holes in the path that tourists can fall through
- **Loop Integration**: Same hazards in same locations each restart

---

## 3. NARRATIVE DESIGN

### 3.1 Story Overview
You are a mountain guide. Your job is simple: get 3 tourists to the top safely.

### 3.2 Simple Narrative
- **Goal**: Reach the summit with all tourists alive
- **Failure**: Any tourist falls = restart
- **Learning**: You remember where dangers are

### 3.3 Characters
- **Guide**: You, the player
- **Tourists**: 3 simple characters that follow you
- **Mountain**: The obstacle course

---

## 4. ART STYLE & AUDIO DESIGN

### 4.1 Visual Style
- **Art Direction**: Simple, clean, readable
- **Colors**: 
  - Green for safe areas
  - Red for dangerous areas
  - Blue for tourists
- **Characters**: Simple geometric shapes
- **Environment**: Basic mountain shapes with clear paths

### 4.2 Audio Design
- **Simple Sounds**:
  - Footsteps
  - Tourist falling sounds
  - Success/failure sounds
- **Music**: One background track

---

## 5. TECHNICAL SPECIFICATIONS

### 5.1 Unity Implementation
- **Physics**: Basic Unity physics for tourists
- **AI**: Simple follow-the-leader for tourists
- **Level**: One static mountain layout
- **Audio**: Basic Unity audio

### 5.2 Performance Targets
- **Frame Rate**: 30 FPS minimum
- **Resolution**: 1280x720
- **Platform**: PC (Windows)
- **Development Time**: 3 days MVP

### 5.3 Core Systems
- **Tourist AI**: Simple follow player
- **Physics**: Basic collision detection
- **Level**: Static mountain with hazards
- **Save**: Just restart functionality

---

## 6. GAMEPLAY FEATURES

### 6.1 Simple Tutorial
- **Controls**: WASD to move, click to grab tourists
- **Goal**: Get all tourists to the top
- **Failure**: Any tourist falls = restart

### 6.2 No Progression
- **Same Level**: Always the same mountain
- **No Upgrades**: Just your basic abilities
- **Learning**: You learn the hazards through repetition

### 6.3 Simple Replayability
- **Same Challenge**: Identical mountain each time
- **Player Learning**: You get better at avoiding hazards
- **Speed Running**: Try to complete it faster

---

## 7. SUCCESS METRICS

### 7.1 Simple Goals
- **Completion**: Players can reach the top
- **Retry**: Players want to try again
- **Fun**: Players enjoy the simple mechanics

### 7.2 Technical Goals
- **Runs**: Game launches without crashes
- **Controls**: Movement and clicking work
- **Physics**: Tourists can fall and be grabbed

---

## 8. DEVELOPMENT ROADMAP

### 8.1 Day 1: Basic Setup
- Player movement (WASD)
- Simple mountain level
- Tourist following AI
- Basic physics

### 8.2 Day 2: Core Gameplay
- Hazards (rocks, ice, gaps)
- Tourist falling mechanics
- Click to grab tourists
- Win/lose conditions

### 8.3 Day 3: Polish
- Simple UI (restart button)
- Basic sounds
- Bug fixes
- Testing

---

## 9. RISK ASSESSMENT

### 9.1 Simple Risks
- **Physics**: Tourists might get stuck
- **AI**: Tourists might not follow properly
- **Scope**: Too many features for 3 days

### 9.2 Simple Solutions
- **Keep it simple**: Basic physics only
- **Simple AI**: Just follow the player
- **Minimal features**: Focus on core loop only

---

## 10. CONCLUSION

"Don't Fall Behind" is a simple physics-based puzzle game where you guide 3 tourists up a mountain. The loop theme is simple: restart with the same level and learn from your mistakes. Perfect for a 3-day game jam with minimal scope and maximum fun. 