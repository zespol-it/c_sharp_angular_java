package com.example.budgetmanager.model;

import jakarta.persistence.*;

@Entity
public class SavingGoal {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    
    private String name;
    private Double targetAmount;
    
    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;
    
    // Getters and setters
    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    public Double getTargetAmount() { return targetAmount; }
    public void setTargetAmount(Double targetAmount) { this.targetAmount = targetAmount; }
    public User getUser() { return user; }
    public void setUser(User user) { this.user = user; }
} 