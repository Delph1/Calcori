import { useEffect, useState } from 'react';
import './App.css';

interface MealItem {
    id: number;
    userMealId: number;
    foodName: string;
    caloriPer100g: number;
    weight: number;
    totalCalories: number;
}

interface UserMeal {
    id: number;
    name: string;
    date: string;
    totalCalories: number;
    mealItems: MealItem[];
}

function MealCard({ meal, onRefresh }: { meal: UserMeal; onRefresh: () => void }) {
    const [foodName, setFoodName] = useState<string>('');
    const [calories, setCalories] = useState<number>(0);
    const [weight, setWeight] = useState<number>(0);

    const deleteMeal = async () => {
        if (!window.confirm('Are you sure you want to delete ${meal.name}?')) return;
        try {
            const response = await fetch(`UserMeals/${meal.id}`, { method: 'DELETE' });
            if (response.ok) onRefresh();
        } catch (error) {
            console.error('Error deleting meal:', error);
        }
    };

    const deleteMealItem = async (itemId: number) => {
        try {
            const response = await fetch(`UserMeals/${meal.id}/MealItems/${itemId}`, { method: 'DELETE' });
            if (response.ok) onRefresh();
        } catch (error) {
            console.error('Error deleting meal item:', error);
        }
    };

    const addMealItem = async (e: React.FormEvent) => {
        e.preventDefault();
        const newItem = {
            userMealId: meal.id,
            foodName,
            caloriPer100g: Number(calories),
            weight: Number(weight)
        };

        try {
            const response = await fetch(`UserMeals/${meal.id}/MealItems`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newItem)
            });

            if (response.ok) {
                setFoodName('');
                setCalories(0);
                setWeight(0);
                onRefresh();
            }
        } catch (error) {
            console.error('Error adding meal item:', error);
        }
    };

    return (
        <div className="meal-card">
            <div className="meal-header">
                <h3>{new Date(meal.date).toLocaleDateString()} - {meal.name}</h3>
                <button onClick={deleteMeal} className="btn-danger">Delete Meal</button>
            </div>
            <p>Total Calories: {meal.totalCalories} kcal</p>
            <ul>
                {meal.mealItems.map(item => (
                    <li key={item.id} className="meal-item">
                        <span>{item.foodName} - {item.weight}g - {item.totalCalories} kcal</span>
                        <button onClick={() => deleteMealItem(item.id)} className="btn-danger btn-small">Delete</button>
                    </li>
                ))}
            </ul>

            <form onSubmit={addMealItem} className="flex-form">
                <input type="text" placeholder="Food Name" value={foodName} onChange={e => setFoodName(e.target.value)} required />
                <input type="number" placeholder="Kcal/100g" value={calories} onChange={e => setCalories(Number(e.target.value))} required />
                <input type="number" placeholder="Weight (g)" value={weight} onChange={e => setWeight(Number(e.target.value))} required />
                <button type="submit" className="btn-success">Add Item</button>
            </form>
        </div>
    );
}

function App() {
    const [meals, setMeals] = useState<UserMeal[]>([]);
    const [loading, setLoading] = useState<boolean>(true);

    const [newMealName, setNewMealName] = useState<string>('');
    const [newMealDate, setNewMealDate] = useState<string>('');

    const fetchMealsData = async () => {
        const response = await fetch('/UserMeals');
        if (response.ok) {
            return await response.json();
        }
        throw new Error('Failed to fetch meals');
    }

    const fetchMeals = async () => {
        try {
            const data = await fetchMealsData();
            setMeals(data);
        } catch (error) {
            console.error('Error fetching meals:', error);
        }
    };

    useEffect(() => {
        fetchMealsData().then(data => {
            setMeals(data);
            setLoading(false);
        }).catch(error => {
            console.error('Error fetching meals:', error);
            setLoading(false);
        });
    }, []);

    const addMeal = async (e: React.FormEvent) => {
        e.preventDefault();
        const newMeal = {
            name: newMealName,
            date: new Date(newMealDate).toISOString()
        };

        try {
            const response = await fetch('/UserMeals', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newMeal)
            });

            if (response.ok) {
                setNewMealName('');
                setNewMealDate('');
                fetchMeals();
            }
        } catch (error) {
            console.error('Error adding meal:', error);
        }
    };



    if (loading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="App">
            <h1>Calcori</h1>

            <div className="add-meal-container">
                <h2>Add New Meal</h2>
                <form onSubmit={addMeal} style={{ display: 'flex', gap: '10px', alignItems: 'center' }}>
                    <input type="text" placeholder="Meal Name" value={newMealName} onChange={e => setNewMealName(e.target.value)} required />
                    <input type="date" value={newMealDate} onChange={e => setNewMealDate(e.target.value)} required />
                    <button type="submit" className="btn-primary">Add Meal</button>
                </form>
            </div>

            {meals.length === 0 ? (
                <p>No meals found.</p>
            ) : (
                meals.map(meal => (
                    <MealCard key={meal.id} meal={meal} onRefresh={fetchMeals} />
                ))
            )}
        </div>
    );
}

export default App;