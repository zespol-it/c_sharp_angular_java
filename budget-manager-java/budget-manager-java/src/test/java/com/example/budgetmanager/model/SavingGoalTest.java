package com.example.budgetmanager.model;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

public class SavingGoalTest {
    @Test
    void testGettersAndSetters() {
        SavingGoal goal = new SavingGoal();
        goal.setId(1L);
        goal.setName("Vacation");
        goal.setTargetAmount(5000.0);
        User user = new User();
        user.setId(2L);
        goal.setUser(user);

        assertEquals(1L, goal.getId());
        assertEquals("Vacation", goal.getName());
        assertEquals(5000.0, goal.getTargetAmount());
        assertEquals(2L, goal.getUser().getId());
    }
} 