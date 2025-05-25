package com.example.budgetmanager.model;

import org.junit.jupiter.api.Test;
import java.time.LocalDate;
import static org.junit.jupiter.api.Assertions.*;

public class TransactionTest {
    @Test
    void testGettersAndSetters() {
        Transaction transaction = new Transaction();
        transaction.setId(1L);
        transaction.setAmount(100.0);
        transaction.setDate(LocalDate.of(2024, 5, 25));
        transaction.setDescription("Test transaction");
        User user = new User();
        user.setId(2L);
        Category category = new Category();
        category.setId(3L);
        transaction.setUser(user);
        transaction.setCategory(category);

        assertEquals(1L, transaction.getId());
        assertEquals(100.0, transaction.getAmount());
        assertEquals(LocalDate.of(2024, 5, 25), transaction.getDate());
        assertEquals("Test transaction", transaction.getDescription());
        assertEquals(2L, transaction.getUser().getId());
        assertEquals(3L, transaction.getCategory().getId());
    }
} 