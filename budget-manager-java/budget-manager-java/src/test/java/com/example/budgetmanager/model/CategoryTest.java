package com.example.budgetmanager.model;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

public class CategoryTest {
    @Test
    void testGettersAndSetters() {
        Category category = new Category();
        category.setId(1L);
        category.setName("Test Category");
        category.setType("Income");
        User user = new User();
        user.setId(2L);
        category.setUser(user);

        assertEquals(1L, category.getId());
        assertEquals("Test Category", category.getName());
        assertEquals("Income", category.getType());
        assertEquals(2L, category.getUser().getId());
    }
} 