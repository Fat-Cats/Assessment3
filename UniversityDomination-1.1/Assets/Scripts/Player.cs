﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public List <Sector> ownedSectors;
    public List <Unit> units;

    //========code by fred===========
    [SerializeField] private bool moving = false;
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private Sector selectedSector;
    //===============================

    [SerializeField] private Game game;
    [SerializeField] private GameObject unitPrefab;
	[SerializeField] private PlayerUI gui;
    [SerializeField] private int beer = 0;
    [SerializeField] private int knowledge = 0;
    [SerializeField] private Color color;
    [SerializeField] private bool human;
    [SerializeField] private bool active = false;


    public Game GetGame() {
        return game;
    }

    public void SetGame(Game game) {
        this.game = game;
    }

    public GameObject GetUnitPrefab() {
        return unitPrefab;
    }

	public PlayerUI GetGui() {
		return gui;
	}

	public void SetGui(PlayerUI gui) {
		this.gui = gui;
	}

    public int GetBeer() {
        return beer;
    }

    public void SetBeer(int beer) {
        this.beer = beer;
    }

    public int GetKnowledge() {
        return knowledge;
    }

    public void SetKnowledge(int knowledge) {
        this.knowledge = knowledge;
    }

    public Color GetColor() {
        return color;
    }

    public void SetColor(Color color) {
        this.color = color;
    }

    public bool IsHuman() {
        return human;
    }

    public void SetHuman(bool human) {
        this.human = human;
    }

    public bool IsActive() {
        return active;
    }

    public void SetActive(bool active) {
        this.active = active;
    }



    public void Capture(Sector sector) {

        // capture the given sector


        // store a copy of the sector's previous owner
        Player previousOwner = sector.GetOwner();

        // add the sector to the list of owned sectors
        ownedSectors.Add(sector);

        // remove the sector from the previous owner's
        // list of sectors
        if (previousOwner != null)
            previousOwner.ownedSectors.Remove(sector);

        // set the sector's owner to this player
        sector.SetOwner(this);

        // if the sector contains a landmark
        if (sector.GetLandmark() != null)
        {
            Landmark landmark = sector.GetLandmark();

            // remove the landmark's resource bonus from the previous
            // owner and add it to this player
            if (landmark.GetResourceType() == Landmark.ResourceType.Beer)
            {
                this.beer += landmark.GetAmount();
                if (previousOwner != null)
                    previousOwner.beer -= landmark.GetAmount();
            }
            else if (landmark.GetResourceType() == Landmark.ResourceType.Knowledge)
            {
                this.knowledge += landmark.GetAmount();
                if (previousOwner != null)
                    previousOwner.knowledge -= landmark.GetAmount();
            }
        }
    }

    public void SpawnUnits() {

        // spawn a unit at each unoccupied landmark


        // scan through each owned sector
		foreach (Sector sector in ownedSectors) 
		{
            // if the sector contains a landmark and is unoccupied
            if (sector.GetLandmark() != null && sector.GetUnit() == null)
            {
                // instantiate a new unit at the sector
                Unit newUnit = Instantiate(unitPrefab).GetComponent<Unit>();

                // initialize the new unit
                newUnit.Initialize(this, sector);

                // add the new unit to the player's list of units and 
                // the sector's unit parameters
                units.Add(newUnit);
                sector.SetUnit(newUnit);
            }
		}
	}

    public bool IsEliminated() {

        // returns true if the player is eliminated, false otherwise;
        // a player is considered eliminated if it has no units left
        // and does not own a landmark

        if (units.Count == 0 && !OwnsLandmark())
            return true;
        else
            return false;
    }

    private bool OwnsLandmark() {

        // returns true if the player owns at least one landmark,
        // false otherwise

        
        // scan through each owned sector
        foreach (Sector sector in ownedSectors)
        {
            // if a landmarked sector is found, return true
            if (sector.GetLandmark() != null)
                return true;
        }

        // otherwise, return false
        return false;
    }

    //=================================code by fred==================================================
    public bool IsMoving()
    {
        return moving;
    }

    public void setMoving(bool moving)
    {
        this.moving = moving;
    }

    public void ComputerTurn()
    {
        StartCoroutine( playComputerMove() );

        //selectedUnit = units[Random.Range(0, units.Count)];                                                                                         // Chooses a random unit that can move
        //selectedUnit.Select();
        //selectedSector = selectedUnit.GetSector().GetAdjacentSectors()[Random.Range(0, selectedUnit.GetSector().GetAdjacentSectors().Length)];      // Chooses a random sector that the selected unit can move into
        //if (selectedSector.GetUnit() == null)
        //{
        //    selectedSector.MoveIntoUnoccupiedSector(selectedUnit);
        //}
        //// if the sector is occupied by a friendly unit
        //else if (selectedSector.GetUnit().GetOwner() == selectedUnit.GetOwner())
        //{
        //    selectedSector.MoveIntoFriendlyUnit(selectedUnit);
        //}
        //// if the sector is occupied by a hostile unit
        //else if (selectedSector.GetUnit().GetOwner() != selectedUnit.GetOwner())
        //{
        //    selectedSector.MoveIntoHostileUnit(selectedUnit, this.selectedSector.GetUnit());
        //}
        //setMoving(false);
    }

    public void Update()
    {
        if (IsHuman() == false && IsActive() && IsMoving() == false)
        {
            setMoving(true);
            ComputerTurn();
        }
    }

    IEnumerator playComputerMove()
    {
        selectedUnit = units[Random.Range(0, units.Count)];                                                                                         // Chooses a random unit that can move
        selectedUnit.Select();

        yield return new WaitForSeconds(1);

        selectedUnit.Deselect();

        selectedSector = selectedUnit.GetSector().GetAdjacentSectors()[Random.Range(0, selectedUnit.GetSector().GetAdjacentSectors().Length)];      // Chooses a random sector that the selected unit can move into
        if (selectedSector.GetUnit() == null)
        {
            selectedSector.MoveIntoUnoccupiedSector(selectedUnit);
        }
        // if the sector is occupied by a friendly unit
        else if (selectedSector.GetUnit().GetOwner() == selectedUnit.GetOwner())
        {
            selectedSector.MoveIntoFriendlyUnit(selectedUnit);
        }
        // if the sector is occupied by a hostile unit
        else if (selectedSector.GetUnit().GetOwner() != selectedUnit.GetOwner())
        {
            selectedSector.MoveIntoHostileUnit(selectedUnit, this.selectedSector.GetUnit());
        }
        setMoving(false);
    }

    //================================================================================================
}