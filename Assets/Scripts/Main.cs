using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlanetsInfo;

public class Main : MonoBehaviour
{
    private Planets[] plants;
    public GameObject BaseBody;
    private double AU = 1.496 * Mathf.Pow(10, 11);
    public long DistanceScale = 1000000000;
    public float SizeScale = 10000;
    public double TimeScale = 0.0001;
    public float TimeStep = 0.0002f;
    private int Voffset = 100000000;

    // Start is called before the first frame update
    void Start()
    {
        Planets Sun = new Planets("Sun", 0, 0, 0, 1.989 * Mathf.Pow(10, 30), 696340, -100000, 0, -1600000);
        Planets Mercury = new Planets("Mercury", 0.39 * AU, 0, 0, 3.285 * Mathf.Pow(10, 23), 2439.7, 0, 0, 47.87 * Voffset);
        Planets Venus = new Planets("Venus", 0.723 * AU, 0, 0, 4.867 * Mathf.Pow(10, 24), 6051.8, 0, 0, 35.02 * Voffset);
        Planets Earth = new Planets("Earth", 1 * AU, 0, 0, 5.972 * Mathf.Pow(10, 24), 6371, 0, 0, 29.78 * Voffset);
        Planets Mars = new Planets("Mars", 1.524 * AU, 0, 0, 6.39 * Mathf.Pow(10, 23), 3389.5, 0, 0, 24.07 * Voffset);
        Planets Jupiter = new Planets("Jupiter", 5.203 * AU, 0, 0, 1.898 * Mathf.Pow(10, 27), 69911, 0, 0, 13.07 * Voffset);
        Planets Saturn = new Planets("Saturn", 9.537 * AU, 0, 0, 5.683 * Mathf.Pow(10, 26), 58232, 0, 0, 9.69 * Voffset);
        Planets Uranus = new Planets("Uranus", 19.191 * AU, 0, 0, 8.681 * Mathf.Pow(10, 25), 25362, 0, 0, 6.81 * Voffset);
        Planets Neptune = new Planets("Neptune", 30.069 * AU, 0, 0, 1.024 * Mathf.Pow(10, 26), 24622, 0, 0, 5.43 * Voffset);
        Planets Pluto = new Planets("Pluto", 39.482 * AU, 0, 0, 1.309 * Mathf.Pow(10, 22), 1188.3, 0, 0, 4.67 * Voffset);
        Planets Moon = new Planets("Moon", 1.00257 * AU, 0, 0, 7.342 * Mathf.Pow(10, 22), 1737.1, 0, 0, 29.65 * Voffset);
        
        plants = new Planets[] { Sun, Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune, Pluto, Moon};
        foreach (Planets planet in plants)
        {
            SpawnBody(planet);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Time.fixedDeltaTime = TimeStep;
        //Debug.Log("Updates per second: " + 1 / TimeStep);

        //Debug.Log(plants[0].VelocityX + ", " + plants[0].VelocityY + ", " + plants[0].VelocityZ);

        foreach(Planets planet in plants)
        {
            MoveGameObjects(planet);
        }
    }

    private void FixedUpdate()
    {
        RunSimulation();
    }

    private void RunSimulation()
    {
        foreach (Planets planet in plants)
        {
            double[] acceleration = AverageAccelerationBetweenAll(planet);
            planet.VelocityX += acceleration[0] * TimeScale;
            planet.VelocityY += acceleration[1] * TimeScale;
            planet.VelocityZ += acceleration[2] * TimeScale;
            UpdatePosition(planet);
        }
    }

    void MoveGameObjects(Planets currentp)
        {
        //find the gameObject for the planet and update its position
        GameObject currentgb = GameObject.Find(currentp.Name);
            

            currentgb.transform.position = new Vector3((float)currentp.PositionX / DistanceScale, (float)currentp.PositionY / DistanceScale, (float)currentp.PositionZ / DistanceScale);
        }

    void SpawnBody(Planets Spawn)
    {
        GameObject Body = Instantiate(BaseBody, new Vector3((float)Spawn.PositionX / DistanceScale, (float)Spawn.PositionY / DistanceScale, (float)Spawn.PositionZ / DistanceScale), Quaternion.identity);
        
        Color randomColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 1f, 1f);
        Body.GetComponent<Renderer>().material.color = randomColor;

        // Use a logarithmic scale for the size
        float logScale = Mathf.Log((float)Spawn.Radius) * SizeScale;
        Body.transform.localScale = new Vector3(logScale, logScale, logScale);
        Body.name = Spawn.Name;

        //Debug.Log("Spawned: " + Spawn.Name + " at " + Spawn.PositionX + ", " + Spawn.PositionY + ", " + Spawn.PositionZ + " with mass: " + Spawn.Mass + " and radius: " + Spawn.Radius + " and velocity: " + Spawn.VelocityX + ", " + Spawn.VelocityY + ", " + Spawn.VelocityZ);
    }

    double[] FindDirection(Planets planet1, Planets planet2)
    {
        double[] direction = new double[3];
        direction[0] = planet2.PositionX - planet1.PositionX;
        direction[1] = planet2.PositionY - planet1.PositionY;
        direction[2] = planet2.PositionZ - planet1.PositionZ;
        return direction;
    }

    double FindAcceleration(double gravity, double mass)
    {
        return gravity / mass;
    }

    double[] AverageAccelerationBetweenAll(Planets planet)
{
    double[] acceleration = new double[3];
    for (int i = 0; i < plants.Length; i++)
    {
        Planets otherPlanet = plants[i];
        if (otherPlanet.Name != planet.Name)
        {
            double gravity = FindGravity(planet, otherPlanet);
            double[] direction = FindDirection(planet, otherPlanet);
            double accelerationBetween = FindAcceleration(gravity, planet.Mass);
            acceleration[0] += accelerationBetween * direction[0];
            acceleration[1] += accelerationBetween * direction[1];
            acceleration[2] += accelerationBetween * direction[2];
        }
    }
    double normalizationFactor = 1.0 / (plants.Length - 1);
    acceleration[0] *= normalizationFactor;
    acceleration[1] *= normalizationFactor;
    acceleration[2] *= normalizationFactor;
    return acceleration;
}

double FindGravity(Planets planet1, Planets planet2)
{
    double distanceSquared = Mathf.Pow((float)(planet1.PositionX - planet2.PositionX), 2) +
                             Mathf.Pow((float)(planet1.PositionY - planet2.PositionY), 2) +
                             Mathf.Pow((float)(planet1.PositionZ - planet2.PositionZ), 2);
    double gravity = G * planet1.Mass * planet2.Mass / distanceSquared;
    return gravity;
}

// Precompute gravitational constant
const double G = 6.674 * 1e-11;


    void UpdatePosition(Planets planet)
    {
        planet.PositionX += planet.VelocityX * TimeScale;
        planet.PositionY += planet.VelocityY * TimeScale;
        planet.PositionZ += planet.VelocityZ * TimeScale;
    }
}
